using Microsoft.AspNetCore.Mvc;
using RideshareApp.Services.Infrastructure.Services;

namespace API.Controllers.Common
{
    public abstract class BaseAsyncProtectiveController<TInDTO, TOutDTO, TEntity, TPk> : ControllerBase
        where TPk : struct
    {
        protected IBaseAsyncService<TInDTO, TOutDTO, TEntity, TPk> baseService;

        protected BaseAsyncProtectiveController(IBaseAsyncService<TInDTO, TOutDTO, TEntity, TPk> baseService)
        {
            this.baseService = baseService;
        }

        [HttpGet("")]
        protected virtual IActionResult GetAll()
        {
            var result = this.baseService.SelectAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get element by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        protected virtual async Task<IActionResult> Get(TPk id)
        {
            var result = await this.baseService.SelectAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Insert data for element
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        [HttpPost("")]
        protected virtual async Task<IActionResult> Post([FromBody] TInDTO inDto)
        {
            var id = await this.baseService.InsertAsync(inDto);
            return Ok(new { id });
        }

        /// <summary>
        /// Update data for element
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inDto"></param>
        /// <returns></returns>
        [HttpPut("{aId}")]
        protected virtual async Task<IActionResult> Put(TPk id, [FromBody] TInDTO inDto)
        {
            await this.baseService.UpdateAsync(id, inDto);
            return Ok();
        }

        /// <summary>
        /// Delete data for element
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        protected virtual async Task<IActionResult> Delete(TPk id)
        {
            await this.baseService.DeleteAsync(id);
            return Ok();
        }
    }
}
