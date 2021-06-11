using System;
using Moba.Services.Interfaces.RoleManager;
using System.Linq;
using System.Threading.Tasks;
using Moba.Data.EF.Interfaces.UOW;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Moba.Common.ViewModels.Security.RoleManager;
using Moba.Domain.Entities.Setting;

namespace Moba.Services.Services.RoleManager
{
    public class RoleManagerHelper : IRoleManagerHelper
    {
        private readonly IUnitOfWork _context;
        public RoleManagerHelper(IUnitOfWork context)
        {
            _context = context;
        }
        public IList<ActionAndControllerNameViewModel> AreaAndActionAndControllerNamesList(Assembly assembly)
        {
            var contradistinction = assembly.GetTypes()
                 .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type =>
                     type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => new
                {
                    Controller = x.DeclaringType?.Name,
                    Action = x.Name,
                    Area = x.DeclaringType?.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))
                });

            var list = new List<ActionAndControllerNameViewModel>();

            foreach (var item in contradistinction)
            {
                if (item.Area.Any())
                {
                    list.Add(new ActionAndControllerNameViewModel()
                    {
                        ControllerName = item.Controller,
                        ActionName = item.Action,
                        AreaName = item.Area.Select(v => v.ConstructorArguments[0].Value?.ToString()).FirstOrDefault()
                    });
                }
                else
                {
                    list.Add(new ActionAndControllerNameViewModel()
                    {
                        ControllerName = item.Controller,
                        ActionName = item.Action,
                        AreaName = null,
                    });
                }
            }

            return list.Distinct().ToList();
        }
        public IList<string> GetAllAreasNames()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var contradistinction = assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type =>
                    type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => new
                {
                    Area = x.DeclaringType?.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))

                });

            var list = contradistinction.Select(item => item.Area.Select(v => v.ConstructorArguments[0].Value?.ToString()).FirstOrDefault()).ToList();

            if (list.All(string.IsNullOrEmpty))
            {
                return new List<string>();
            }

            list.RemoveAll(x => x == null);

            return list.Distinct().ToList();
        }

        public async Task<string> DataBaseRoleValidationGuid()
        {
            var roleValidationGuid =
               await _context.SettingRoleManagerRepository.GetFirstAsync(s => s.Id == "RoleValidationGuid");

            while (roleValidationGuid == null)
            {
                await _context.SettingRoleManagerRepository.InsertAsync(new SettingRoleManager()
                {
                    Id = "RoleValidationGuid",
                    Value = Guid.NewGuid().ToString(),
                    RegisterDate = DateTime.Now
                });

                await _context.SaveChangesAsync();

                roleValidationGuid =
                   await _context.SettingRoleManagerRepository.GetFirstAsync(s => s.Id == "RoleValidationGuid");
            }
            return roleValidationGuid.Value;
        }
    }
}