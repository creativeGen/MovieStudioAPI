using CsvHelper;
using MovieStudioAPI.Models;
using MovieStudioAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStudioAPI.Repository
{
    public interface IMovieStudioRepository
    {
        void AddMovieMetadata(MetadataDTO movieMetadata);
        List<MetadataDTO> GetMovieById(int movieId);
        List<StatsDTO> GetMovieStats();

    }
    public class MovieStudioRepository : IMovieStudioRepository
    {
        private List<MetadataDTO> _listDatabase;

        public MovieStudioRepository()
        {
            _listDatabase = new List<MetadataDTO>();
        }
        public void AddMovieMetadata(MetadataDTO movieMetadata)
        {
            _listDatabase.Add(
                new MetadataDTO
                {
                    MovieId = movieMetadata.MovieId,
                    Title = movieMetadata.Title,
                    Language = movieMetadata.Language,
                    Duration = movieMetadata.Duration,
                    ReleaseYear = movieMetadata.ReleaseYear
                }
            );
        }

        public List<MetadataDTO> GetMovieById(int movieId)
        {
            var _listMetadata = new List<Metadata>();
            _listMetadata = ReadMovieMetadataFromCsv();
            var moviesById = from x in _listMetadata
                             where x.MovieId == movieId
                             group x by x.Language
                            into groups
            select groups.OrderByDescending(p => p.Id).First();
            var listMetadataDTO = new List<MetadataDTO>();

            foreach (var movie in moviesById)
            {
                if(string.IsNullOrEmpty(movie.Id.ToString()) || string.IsNullOrEmpty(movie.MovieId.ToString()) 
                    || string.IsNullOrEmpty(movie.Language) || string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.ReleaseYear.ToString()))
                {
                    continue;
                }
                listMetadataDTO.Add(
                    new MetadataDTO()
                    {
                        MovieId = movie.MovieId,
                        Title = movie.Title,
                        Language = movie.Language,
                        Duration = movie.Duration,
                        ReleaseYear = movie.ReleaseYear
                    });
            }
            return listMetadataDTO;
        }

        public List<StatsDTO> GetMovieStats()
        {
            var _listMetadata = new List<Metadata>();
            var _listMovieStatsCSV = new List<Stats>();

            _listMovieStatsCSV = ReadMovieStatsFromCsv();
            _listMetadata = ReadMovieMetadataFromCsv();

            var result = _listMovieStatsCSV
                        .GroupBy(x => x.movieId)
                        .Select(g => new {
                            MovieId = g.Key,
                            Watches = g.Count(),
                            WatchesInMS = g.Sum(x => x.watchDurationMs > 0 ? x.watchDurationMs : 0)
                        }).ToList();
            var moviesByReleaseYear = from x in _listMetadata
                                      group x by x.MovieId
                                       into groups
                                      select groups.First();
                             //select groups.OrderByDescending(p => p.Id).First();

            var output = from movieStat in result
                         join movieMetadata in moviesByReleaseYear
                         on movieStat.MovieId equals movieMetadata.MovieId
                         orderby movieStat.Watches descending, movieMetadata.ReleaseYear descending
                         select new StatsDTO
                         {
                             MovieId = movieStat.MovieId,
                             Title = movieMetadata.Title,
                             AverageWatchDurationS = movieStat.WatchesInMS>0 ? Math.Round(movieStat.WatchesInMS / movieStat.Watches / 1000) : 0,
                             Watches = movieStat.Watches,
                             ReleaseYear = movieMetadata.ReleaseYear
                         };
            return output.ToList();
        }
        private List<Metadata> ReadMovieMetadataFromCsv()
        {
            try
            {
                var fileName = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Data\\metadata.csv");
                using (var csv = new CsvReader(new StreamReader(fileName, Encoding.Default), CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Metadata>().ToList();
                    return records;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private List<Stats> ReadMovieStatsFromCsv()
        {
            try
            {
                var fileName = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Data\\stats.csv");
                using (var csv = new CsvReader(new StreamReader(fileName, Encoding.Default), CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Stats>().ToList();
                    return records;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
