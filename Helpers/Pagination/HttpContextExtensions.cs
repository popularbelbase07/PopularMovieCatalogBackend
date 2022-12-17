using Microsoft.EntityFrameworkCore;

namespace PopularMovieCatalogBackend.Helpers.Pagination
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametsrPaginationInHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Add("totalamountofrecords", count.ToString());

        }
    }
}
