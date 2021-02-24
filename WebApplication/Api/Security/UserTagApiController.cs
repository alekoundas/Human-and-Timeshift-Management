using Bussiness;
using DataAccess;
using DataAccess.Models.Security;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApplication.Api.Security
{
    [Route("api/usertags")]
    [ApiController]
    public class UserTagApiController : ControllerBase
    {
        private SecurityDbContext _securityDbContext;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public UserTagApiController(SecurityDbContext securityDbContext, BaseDbContext baseDbContext)
        {
            _securityDbContext = securityDbContext;
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _baseDataWork = new BaseDatawork(baseDbContext);

        }

        //POST api/userrole/updateworkplaceroles
        [HttpPost("UpdateUserTags")]
        public async Task<ActionResult> UpdateUserTags([FromBody] UserTagsUpdate viewModel)
        {
            //Delete old user tags - so i can add new
            var userTagsToDelete = await _securityDatawork.ApplicationUserTags.GetFromUserId(viewModel.UserId);
            _securityDatawork.ApplicationUserTags.RemoveRange(userTagsToDelete);
            await _securityDatawork.SaveChangesAsync();

            foreach (var tagTitle in viewModel.Values)
            {
                ApplicationTag tag;
                //If tag exists in db
                if (_securityDatawork.ApplicationTags.Any(x => x.Title == tagTitle))
                    tag = await _securityDatawork.ApplicationTags
                        .FirstOrDefaultAsync(x => x.Title == tagTitle);
                else
                {
                    //Create new Tag then
                    tag = new ApplicationTag()
                    {
                        Title = tagTitle,
                        CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                        CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                        CreatedOn = DateTime.Now
                    };

                    //Save to generate Id
                    _securityDatawork.ApplicationTags.Add(tag);
                    await _securityDatawork.SaveChangesAsync();
                }

                _securityDatawork.ApplicationUserTags.Add(new ApplicationUserTag
                {
                    ApplicationUserId = viewModel.UserId,
                    ApplicationTagId = tag.Id
                });
                await _securityDatawork.SaveChangesAsync();

            }

            return Ok();
        }
    }
}
