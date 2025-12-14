using ArxOne.MrAdvice.Advice;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
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

        var cacheKeyUnique = ComposeCacheKeyUnique(context);

        var valueFromCache = await GetValueFromCache(cacheKeyUnique).ConfigureAwait(false);

        if (valueFromCache is not null)
        {
            context.ReturnValue = Task.FromResult(valueFromCache);
            return;
        }
        
        await context.ProceedAsync();

        await PersistToCache(context.ReturnValue, cacheKeyUnique).ConfigureAwait(false);
    }

    #region Private Methods

    private async Task PersistToCache(object? returnValue, string cacheKeyUnique)
    {
        var value = returnValue?.GetType()?
                    .GetProperty("Result")?
                    .GetValue(returnValue);
        var jsonValue = JsonSerializer.Serialize(value);

        Debug.Assert(_memoryCacheService is not null, "MemoryCacheService is null");
        await _memoryCacheService.Set<string>(cacheKeyUnique, jsonValue, TimeSpan.FromHours(1)).ConfigureAwait(false);
    }

    private async Task<object?> GetValueFromCache(string cacheKeyUnique)
    {
        Debug.Assert(_memoryCacheService is not null, "MemoryCacheService is null");
        if (await _memoryCacheService
            .TryGetValue<string>(cacheKeyUnique, out var cachedValue)
            .ConfigureAwait(false))
        {
            var valueObject = !string.IsNullOrEmpty(cachedValue)
                                            ? JsonSerializer.Deserialize(cachedValue, ReturnType)
                                            : null;
            return valueObject;
        }

        return null;
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