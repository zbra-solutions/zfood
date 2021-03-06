using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZFood.Core.API;
using ZFood.Web.DTO;
using ZFood.Web.Extensions;

namespace ZFood.Web.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class RestaurantsController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RestaurantsController));

        private readonly IRestaurantService service;

        public RestaurantsController(IRestaurantService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Searches for restaurants
        /// </summary>
        /// <param name="take">Number of restaurants to take</param>
        /// <param name="skip">Number of restaurants to skip</param>
        /// <param name="query">Query</param>
        /// <param name="count">Count restaurants</param>
        /// <response code="200">
        /// Returned code when a page, with the given parameters, can be build successfully
        /// </response>
        /// <response code="400">
        /// Returned code when skip and take parameters are invalid. Note that skip and take must be greater than zero
        /// </response>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PageDTO<RestaurantDTO>>> Get(int take, int skip, bool count = false, string query = null)
        {
            var pageRequest = BuildPageRequest(take, skip, count, query);
            var page = await service.Get(pageRequest);
            return page.ToDTO(r => r.ToDTO());
        }

        private PageRequest BuildPageRequest(int take, int skip, bool count, string query)
        {
            return new PageRequest
            {
                Skip = skip,
                Take = take,
                Count = count,
                Query = query
            };
        }

        // GET restaurants/5
        /// <summary>
        ///  Finds a Restaurant by Id
        /// </summary>
        /// <param name="id">Id of the Restaurant to be found</param>
        /// <response code="200">
        /// Returned code when a Restaurant with the specified Id can be found
        /// </response>
        /// <response code="404">
        /// Returned code when a Restaurant with the specified Id cannot be found
        /// </response>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetRestaurant")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RestaurantDTO>> Get(string id)
        {
            var restaurant = await service.FindById(id);

            if (restaurant == null)
            {
                log.Debug($"Restaurant {id} was not found");
                return NotFound();
            }
            return restaurant.ToDTO();
        }

        // POST restaurants
        /// <summary>
        /// Creates a Restaurant with the given data
        /// </summary>
        /// <param name="dto">Contains the necessary fields to create a Restaurant</param>
        /// <response code="201">
        /// Returned code when the Restaurant can be created successfully
        /// </response>
        /// <response code="400">
        /// Returned code when trying to create a new Restaurant with the same values of an already existing restaurant
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody] CreateRestaurantRequestDTO dto)
        {
            var createdRestaurant = await service.CreateRestaurant(dto.FromDTO());
            return CreatedAtRoute("GetRestaurant", new { id = createdRestaurant.Id }, createdRestaurant.ToDTO());
        }

        // PUT restaurants/5
        /// <summary>
        /// Updates a Restaurant with the given data
        /// </summary>
        /// <param name="id">Id of the Restaurant to be updated.</param>
        /// <param name="dto">Contains the data of the Restaurant to be changed</param>
        /// <response code="204">
        /// Returned code when the Restaurant can be updated successfully
        /// </response>
        /// <response code="404">
        /// Returned code when cannot find the Restaurant to be updated with the given Id
        /// </response>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put(string id, [FromBody] UpdateRestaurantRequestDTO dto)
        {
            await service.UpdateRestaurant(dto.FromDTO(id));
            return NoContent();
        }

        // DELETE restaurants/5
        /// <summary>
        /// Deletes a Restaurant by Id
        /// </summary>
        /// <param name="id">Id of the Restaurant to be deleted</param>
        /// <response code="204">
        /// Returned code when a Restaurant can be deleted successfully
        /// </response>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(string id)
        {
            await service.Delete(id);
            return NoContent();
        }
    }
}
