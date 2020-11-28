using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Extension;
using MongoDB.Resource;
using MongoDB.Services;
using System;
using System.Collections.Generic;

namespace MongoDB.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class SavedAttrController : Controller
    {
        private readonly SavedBuildingAttService _savedBuildingAttService;
        
       
     
        public SavedAttrController(SavedBuildingAttService savedBuildingAttService)
        {
            _savedBuildingAttService = savedBuildingAttService;
        }
        /// <summary>
        /// 获取所有的属性信息
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAttributes")]
        public ActionResult<Attributes> Get()
        {
            var attribute = _savedBuildingAttService.Get();
        
            return Ok(attribute);
        }
        [HttpHead(Name = "GetAttribute")]
        public List<Attributes> GetAttribute()
        {
            List<Attributes> attributes = new List<Attributes>();
            var attribute = _savedBuildingAttService.Get();
            foreach (var item in attribute)
            {
                if(item.isShow==true)
                {
                    attributes.Add(item);
                }    
            }
            return attributes;
        }
       

        [HttpDelete]
        public IActionResult Delete(string name)
        {
            var Attribute = _savedBuildingAttService.Get(name);
            if (Attribute == null)
            {
                return NotFound();
            }
            _savedBuildingAttService.Remove(Attribute.name);
            return NoContent();
        }
       
        [HttpPut]
        public IActionResult Update(Attributes attributes)
        {
            var attribute = _savedBuildingAttService.Get(attributes.name);
            if(attribute==null)
            {
                return NotFound();
            }
            var match= _savedBuildingAttService.Update(attributes.name, attributes);
            return Ok(match);
        }
        //[HttpPut]
        //public IActionResult<Attributes> Create([FromForm] Attributes attributeAdd)
        //{
        //    if (attributeAdd == null)
        //    {
        //        return BadRequest();
        //    }
        //    var attribute = _savedBuildingAttService.Create(attributeAdd);

        //}
    }
}
