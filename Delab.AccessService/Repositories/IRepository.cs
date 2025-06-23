using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.AccessService.Repositories;
public interface IRepository
{
    // Metodi per file binari
    Task<HttpResponseWrapper<byte[]>> GetFileAsync(string url);
    
    // Metodi GET
    Task<HttpResponseWrapper<T?>> GetAsync<T>(string url) where T : class;
    Task<HttpResponseWrapper<object?>> GetAsync(string url);
    
    // Metodi POST
    Task<HttpResponseWrapper<object?>> PostAsync<TData>(string url, TData model) where TData : class;
    Task<HttpResponseWrapper<TResponse?>> PostAsync<TData, TResponse>(string url, TData model) 
        where TData : class 
        where TResponse : class;
    
    // Metodi PUT
    Task<HttpResponseWrapper<object?>> PutAsync<TData>(string url, TData model) where TData : class;
    Task<HttpResponseWrapper<TResponse?>> PutAsync<TData, TResponse>(string url, TData model)
        where TData : class
        where TResponse : class;
    
    // Metodi DELETE
    Task<HttpResponseWrapper<object?>> DeleteAsync(string url);
    Task<HttpResponseWrapper<TResponse?>> DeleteAsync<TResponse>(string url) where TResponse : class;
}

/*
 * Task<HttpResponseWrapper<byte[]>> GetFileAsync(string url);

    Task<HttpResponseWrapper<object>> GetAsync(string url);
    Task<HttpResponseWrapper<T>> GetAsync<T>(string url) where T : class;


    Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model) ;

    Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model) where TResponse: class where T : class;
    

    Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model);

    Task<HttpResponseWrapper<TResponse>> PutAsync<T, TResponse>(string url, T model) where TResponse : class where T : class;

    Task<HttpResponseWrapper<object>> DeleteAsync(string url);
 */ 