using ArxOne.MrAdvice.Advice;
using Common.Core.Shared.Cache;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Core.AOP.Cache;

public class CacheAttribute : Attribute, IMethodAsyncAdvice
{
    public string MasterKey { get; set; }
    public Type[] CachedTypes { get; set; }
    public Type ReturnType { get; set; }

    private IMemoryCacheService? _memoryCacheService;

    public CacheAttribute(string masterKey, Type returnType, params Type[] cachedTypes)
    {
        MasterKey = masterKey;
        CachedTypes = cachedTypes;
        ReturnType = returnType;
    }

    public async Task Advise(MethodAsyncAdviceContext context)
    {
        var serviceProvider = context.GetMemberServiceProvider();
        _memoryCacheService = serviceProvider?.GetRequiredService<IMemoryCacheService>();

        Debug.Assert(_memoryCacheService is not null, "MemoryCacheService is null");

        var cancellationToken = this.GetCancellationToken(context) ?? CancellationToken.None;
        var cacheKeyUnique = ComposeCacheKeyUnique(context);

        var valueFromCache = await GetValueFromCache(cacheKeyUnique, cancellationToken).ConfigureAwait(false);

        if (valueFromCache is not null)
        {
            context.ReturnValue = Task.FromResult(valueFromCache);
            return;
        }
        
        await context.ProceedAsync();

        await PersistToCache(context.ReturnValue, cacheKeyUnique, cancellationToken).ConfigureAwait(false);
    }

    #region Private Methods

    private async Task PersistToCache(object? returnValue, string cacheKeyUnique, CancellationToken token)
    {
        var value = returnValue?.GetType()?
                    .GetProperty("Result")?
                    .GetValue(returnValue);
        var jsonValue = JsonSerializer.Serialize(value);

        Debug.Assert(_memoryCacheService is not null, "MemoryCacheService is null");
        await _memoryCacheService.Upsert<string>(cacheKeyUnique, jsonValue, TimeSpan.FromHours(1), token).ConfigureAwait(false);
    }

    private async Task<object?> GetValueFromCache(string cacheKeyUnique, CancellationToken token)
    {
        Debug.Assert(_memoryCacheService is not null, "MemoryCacheService is null");
        var result = await _memoryCacheService.GetValue<string>(cacheKeyUnique, token).ConfigureAwait(false);
        if (result.Success)
        {
            var valueObject = !string.IsNullOrEmpty(result.CachedValue)
                                            ? JsonSerializer.Deserialize(result.CachedValue, ReturnType)
                                            : null;
            return valueObject;
        }

        return null;
    }

    private CancellationToken? GetCancellationToken(MethodAsyncAdviceContext context)
    {
        return (CancellationToken)context.Arguments.Single(arg => arg.GetType() == typeof(CancellationToken));
    }

    private string ComposeCacheKeyUnique(MethodAsyncAdviceContext context)
    {
        var cacheKeyUnique = MasterKey;

        foreach (var cachedType in CachedTypes)
        {
            cacheKeyUnique = string.Concat(cacheKeyUnique, "-", GetKeyValue(context.Arguments, cachedType));
        }

        return cacheKeyUnique;
    }

    private string? GetKeyValue(IList<Object> argements, Type cachedType)
    {
        var cachedArgument = argements.Single(arg => arg.GetType() == cachedType);

        if (cachedArgument is null)
        {
            return string.Empty;
        }

        var keyProperty = cachedArgument?.GetType().GetProperties().Single(prop =>
        {
            var cachedKeyAttribute = prop.GetCustomAttribute<CacheKeyAttribute>();
            return cachedKeyAttribute is not null;
        });

        return keyProperty?.GetValue(cachedArgument)?
                           .ToString();
    }

    #endregion
}