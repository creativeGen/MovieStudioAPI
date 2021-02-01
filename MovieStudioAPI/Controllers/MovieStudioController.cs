using Microsoft.AspNetCore.Mvc;
using MovieStudioAPI.DTOs;
using MovieStudioAPI.Services;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieStudioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieStudioController : ControllerBase
    {
        IMovieStudioService _movieStudioService;

        public MovieStudioController(IMovieStudioService movieStudioService)
        {
            _movieStudioService = movieStudioService;
        }

        /// <summary>
        /// This is to get metadata od the movies for the passed movie id.
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns>The list of metadata for the movie id passed. 404 if there was no data for the given movie Id.</returns>
        // GET api/<MovieStudioController>/5
        [HttpGet("metadata/{movieId}")]
        public IActionResult GetMovieById(int movieId)
        {
            try
            {
                if(string.IsNullOrEmpty(movieId.ToString()) || movieId<=0)
                {
                    return BadRequest("Please enter valid movie id.");
                }
                var movies = _movieStudioService.GetMovieById(movieId);
                if (movies != null && movies.Count() > 0)
                {
                    return Ok(movies);
                }
                return NotFound($"MovieId {movieId} does not exist in the database.");
            }
            catch(Exception ex)
            {
                //ex.Message can be logged.
                return StatusCode(500, $"Error retrieving data for movie id {movieId}");
            }
        }

        /// <summary>
        /// This is to get the movie stats.
        /// </summary>
        /// <returns>List of Movie stats. If not results were retrieved, returns 404 and if there was any error retrieving, then returns 500.</returns>
        // GET api/<MovieStudioController>/5
        [HttpGet("movies/stats")]
        public IActionResult GetMovieStats()
        {
            try
            {
                var movieStats = _movieStudioService.GetMovieStats();
                if (movieStats != null && movieStats.Count() > 0)
                {
                    return Ok(movieStats);
                }
                return NotFound($"Movie stats data could not be retrieved.");
            }
            catch(Exception ex)
            {
                //ex.Message can be logged.
                return StatusCode(500, $"Error retrieving stats data for movies.");
            }
        }

        // POST api/<MovieStudioController>
        /// <summary>
        /// This is to add movie metadata
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns>Ok if the data was added else 500 is there was any error.</returns>
        [HttpPost("metadata")]
        public IActionResult Post([FromBody] MetadataDTO metadata)
        {
            try
            {
                _movieStudioService.AddMovieMetadata(metadata);
                return Ok();
            }
            catch(Exception ex)
            {
                //ex.Message can be logged.
                return StatusCode(500, $"Error posting data for movies.");
            }
        }

    }
}
