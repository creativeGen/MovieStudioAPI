using MovieStudioAPI.DTOs;
using MovieStudioAPI.Models;
using MovieStudioAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStudioAPI.Services
{
    public interface IMovieStudioService
    {
        void AddMovieMetadata(MetadataDTO movieMetadata);
        List<MetadataDTO> GetMovieById(int movieId);
        List<StatsDTO> GetMovieStats();

    }
    public class MovieStudioService : IMovieStudioService
    {
        IMovieStudioRepository _movieStudioRepository;
        public MovieStudioService(IMovieStudioRepository movieStudioRepository)
        {
            _movieStudioRepository = movieStudioRepository;
        }
        public void AddMovieMetadata(MetadataDTO movieMetadata)
        {
            _movieStudioRepository.AddMovieMetadata(movieMetadata);
        }

        public List<MetadataDTO> GetMovieById(int movieId)
        {
            return _movieStudioRepository.GetMovieById(movieId);
        }

        public List<StatsDTO> GetMovieStats()
        {
            return _movieStudioRepository.GetMovieStats();
        }
    }
}
