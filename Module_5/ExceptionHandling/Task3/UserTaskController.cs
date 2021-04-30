using System;
using Task3.DoNotChange;

namespace Task3
{
    public class UserTaskController
    {
        private readonly IUserTaskService _taskService;

        public UserTaskController(IUserTaskService taskService)
        {
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        }

        public bool AddTaskForUser(int userId, string description, IResponseModel model)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException($"'{nameof(description)}' cannot be null or empty.", nameof(description));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            string message = GetMessageForModel(userId, description);
            if (message != null)
            {
                model.AddAttribute("action_result", message);
                return false;
            }

            return true;
        }

        private string GetMessageForModel(int userId, string description)
        {
            try
            {
                var task = new UserTask(description);
                _taskService.AddTaskForUser(userId, task);
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is UserNotFoundException)
            {
                return ex.Message;
            }

            return null;
        }
    }
}
