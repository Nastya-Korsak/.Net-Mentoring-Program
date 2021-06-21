using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Core.Model;
using BrainstormSessions.ViewModels;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace BrainstormSessions.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBrainstormSessionRepository _sessionRepository;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));

        public HomeController(IBrainstormSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Debug("Start of Index Method execution");

            var sessionList = await _sessionRepository.ListAsync();

            var model = sessionList.Select(session => new StormSessionViewModel()
            {
                Id = session.Id,
                DateCreated = session.DateCreated,
                Name = session.Name,
                IdeaCount = session.Ideas.Count
            });

            _logger.Info("Model is created");

            _logger.Debug("Finish of Index Method execution");

            return View(model);
        }

        public class NewSessionModel
        {
            [Required]
            public string SessionName { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Index(NewSessionModel model)
        {
            _logger.Debug($"Start of Index Method execution with model {model.SessionName}");

            if (!ModelState.IsValid)
            {
                _logger.Warn("ModelState is not valid");

                return BadRequest(ModelState);
            }
            else
            {
                await _sessionRepository.AddAsync(new BrainstormSession()
                {
                    DateCreated = DateTimeOffset.Now,
                    Name = model.SessionName
                });
            }

            _logger.Debug($"Finish of Index Method execution with model {model.SessionName}");

            return RedirectToAction(actionName: nameof(Index));
        }
    }
}
