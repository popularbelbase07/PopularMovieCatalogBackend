﻿using PopularMovieCatalogBackend.DTOs;

namespace PopularMovieCatalogBackend.Helpers.Pagination
{
    public static class IQueryableExtensions
    {

        public static IQueryable<T> Pagination<T>(this IQueryable<T>queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
                .Take(paginationDTO.RecordsPerPage);
        }
    }
}