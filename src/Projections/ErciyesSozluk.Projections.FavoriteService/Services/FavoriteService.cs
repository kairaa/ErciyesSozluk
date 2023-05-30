﻿using Dapper;
using ErciyesSozluk.Common.Events.Entry;
using ErciyesSozluk.Common.Events.EntryComment;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Projections.FavoriteService.Services
{
    public class FavoriteService
    {
        private readonly string connectionString;

        public FavoriteService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //event c#'a ait isim old için başına @ koyulur
        public async Task CreateEntryFav(CreateEntryFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection
                .ExecuteAsync("INSERT INTO EntryFavorite (Id, EntryId, CreatedById, CreateDate) VALUES(@Id, @EntryId, @CreatedById, GETDATE())", new
                {
                    Id = Guid.NewGuid(),
                    EntryId = @event.EntryId,
                    CreatedById = @event.CreatedBy,
                });
        }

        public async Task CreateEntryCommentFav(CreateEntryCommentFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("INSERT INTO EntryCommentFavorite (Id, EntryCommentId, CreatedById, CreateDate) VALUES(@Id, @EntryCommentId, @CreatedById, GETDATE())",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryCommentId = @event.EntryCommentId,
                    CreatedById = @event.CreatedBy
                });
        }

        public async Task DeleteEntryFav(DeleteEntryFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE FROM EntryFavorite WHERE EntryId = @EntryId AND CreatedById = @CreatedById",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryId = @event.EntryId,
                    CreatedById = @event.CreatedBy
                });
        }

        public async Task DeleteEntryCommentFav(DeleteEntryCommentFavEvent @event)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("DELETE FROM EntryCommentFavorite WHERE EntryCommentId = @EntryCommentId AND CreatedById = @CreatedById",
                new
                {
                    Id = Guid.NewGuid(),
                    EntryCommentId = @event.EntryCommentId,
                    CreatedById = @event.CreatedBy
                });
        }
    }
}
