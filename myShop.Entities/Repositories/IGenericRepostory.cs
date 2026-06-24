using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;

namespace myShop.Entities.Repositories;

public interface IGenericRepostory<T> where T : class
{
    // بتجيبلي .TOLIST
IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null , String? IncludeWord = null);
// بتجيبلي .GetFirstorDefault
T GetFirstorDefault(Expression<Func<T, bool>>? predicate = null , String? IncludeWord = null);

void Add (T entity);

void Remove (T entity);

void RemoveRange (IEnumerable<T> entities);


}
