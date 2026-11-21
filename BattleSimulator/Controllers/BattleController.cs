using BattleSimulator.Battle.Services;
using BattleSimulator.Presets;
using Microsoft.AspNetCore.Mvc;

namespace BattleSimulator.Controllers
{
    public class BattleController : Controller
    {
        private readonly IBattleService _battleService;

       


        public BattleController(IBattleService battleService)
        {
            _battleService = battleService;
        }

        [HttpPost]
        public IActionResult Simulate()
        {
            var fighterA = CharacterPresets.CreateLeonidas();
            var fighterB = CharacterPresets.CreateHarald();

            var result = _battleService.SimulateBattle(fighterA, fighterB);
            return Json(result);
        }

        // Removed until needed
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
