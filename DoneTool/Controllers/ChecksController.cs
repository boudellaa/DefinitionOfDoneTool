// <copyright file="ChecksController.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   This file contains the implementation of the ChecksController class.
// </summary>
namespace DoneTool.Controllers
{
    using AutoMapper;
    using DoneTool.Models.Domain;
    using DoneTool.Models.DTO;
    using DoneTool.Repositories.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChecksController"/> class.
    /// </summary>
    /// <param name="checksRepository">The repository to manage checks.</param>
    /// <param name="mapper">The mapper for mapping between domain models and DTOs.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class ChecksController(IChecksRepository checksRepository, IMapper mapper)
        : ControllerBase
    {
        /// <summary>
        /// Gets a list of all checks.
        /// </summary>
        /// <returns>A list of <see cref="ChecksDTO"/> representing all checks.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChecksDTO>>> GetChecks()
        {
            var checks = await checksRepository.GetAllChecks();
            var checksDTOs = mapper.Map<IEnumerable<ChecksDTO>>(checks);
            return this.Ok(checksDTOs);
        }

        /// <summary>
        /// Gets a specific check by ID.
        /// </summary>
        /// <param name="id">The ID of the check to retrieve.</param>
        /// <returns>The <see cref="ChecksDTO"/> representing the requested check.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ChecksDTO>> GetCheck(Guid id)
        {
            var check = await checksRepository.GetCheckById(id);

            if (check == null)
            {
                return this.NotFound();
            }

            var checkDTO = mapper.Map<ChecksDTO>(check);
            return this.Ok(checkDTO);
        }

        /// <summary>
        /// Creates a new check.
        /// </summary>
        /// <param name="checkDTO">The DTO containing the data for the new check.</param>
        /// <returns>The created <see cref="ChecksDTO"/> with its ID.</returns>
        [HttpPost]
        public async Task<ActionResult<ChecksDTO>> PostCheck(ChecksDTO checkDTO)
        {
            var check = mapper.Map<Checks>(checkDTO);
            await checksRepository.AddCheck(check);

            var createdCheckDTO = mapper.Map<ChecksDTO>(check);
            return this.CreatedAtAction(nameof(this.GetCheck), new { id = check.ID }, createdCheckDTO);
        }

        /// <summary>
        /// Updates an existing check.
        /// </summary>
        /// <param name="id">The ID of the check to update.</param>
        /// <param name="checkDTO">The DTO containing the updated data for the check.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheck(Guid id, ChecksDTO checkDTO)
        {
            var existingCheck = await checksRepository.GetCheckById(id);
            if (existingCheck == null)
            {
                return this.NotFound();
            }

            existingCheck.Item = checkDTO.Item;
            await checksRepository.UpdateCheck(existingCheck);

            return this.NoContent();
        }

        /// <summary>
        /// Deletes a specific check by ID.
        /// </summary>
        /// <param name="id">The ID of the check to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheck(Guid id)
        {
            var check = await checksRepository.GetCheckById(id);
            if (check == null)
            {
                return this.NotFound();
            }

            await checksRepository.DeleteCheck(id);

            return this.NoContent();
        }
    }
}
