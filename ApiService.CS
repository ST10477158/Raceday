using System.Text;
using System.Text.Json;
using RaceDay.Web.ViewModels;
namespace RaceDay.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };
        public ApiService(IHttpClientFactory factory) => _http = factory.CreateClient("RaceDayApi");
        public async Task<(bool Success, int User_ID, string User_Email, string User_Role, int Participant_ID, int Organiser_ID, string DisplayName)> LoginAsync(LoginViewModel m)
        {
            var payload = new { User_Email = m.User_Email, User_Password = m.User_Password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var res = await _http.PostAsync("api/auth/login", content);
            if (!res.IsSuccessStatusCode) return (false, 0, "", "", 0, 0, "");
            var data = JsonSerializer.Deserialize<JsonElement>(await res.Content.ReadAsStringAsync(), _json);
            return (true, data.GetProperty("User_ID").GetInt32(), data.GetProperty("User_Email").GetString()!, data.GetProperty("User_Role").GetString()!, data.TryGetProperty("Participant_ID", out var p) ? p.GetInt32() : 0, data.TryGetProperty("Organiser_ID", out var o) ? o.GetInt32() : 0, data.TryGetProperty("Participant_Name", out var pn) ? pn.GetString()! : data.GetProperty("User_Email").GetString()!);
        }
        public async Task<bool> RegisterAsync(RegisterViewModel m)
        {
            var payload = new { User_Email = m.User_Email, User_Password = m.User_Password, User_Role = m.User_Role };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return (await _http.PostAsync("api/auth/register", content)).IsSuccessStatusCode;
        }
        public async Task<List<EventViewModel>> GetAllEventsAsync()
        {
            var res = await _http.GetAsync("api/events");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<EventViewModel>>(await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        public async Task<EventViewModel?> GetEventAsync(int id)
        {
            var res = await _http.GetAsync($"api/events/{id}");
            if (!res.IsSuccessStatusCode) return null;
            return JsonSerializer.Deserialize<EventViewModel>(await res.Content.ReadAsStringAsync(), _json);
        }
        public async Task<bool> CreateEventAsync(EventViewModel m)
        {
            var payload = new { Event_Name = m.Event_Name, Event_Description = m.Event_Description, Event_Venue = m.Event_Venue, Event_StartTime = m.Event_StartTime };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return (await _http.PostAsync("api/events", content)).IsSuccessStatusCode;
        }
        public async Task<bool> UpdateEventAsync(EventViewModel m)
        {
            var payload = new { Event_Name = m.Event_Name, Event_Description = m.Event_Description, Event_Venue = m.Event_Venue, Event_StartTime = m.Event_StartTime };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return (await _http.PutAsync($"api/events/{m.Event_ID}", content)).IsSuccessStatusCode;
        }
        public async Task<bool> DeleteEventAsync(int id) => (await _http.DeleteAsync($"api/events/{id}")).IsSuccessStatusCode;
        public async Task<List<CategoryViewModel>> GetCategoriesAsync(int eventId)
        {
            var res = await _http.GetAsync($"api/events/{eventId}/categories");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<CategoryViewModel>>(await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        public async Task<bool> AddCategoryAsync(CategoryViewModel m)
        {
            var payload = new { Category_Name = m.Category_Name, Category_EntryAmount = m.Category_EntryAmount, Category_QualifyAge = m.Category_QualifyAge };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return (await _http.PostAsync($"api/events/{m.Event_ID}/categories", content)).IsSuccessStatusCode;
        }
        public async Task<bool> UpdateCategoryAsync(CategoryViewModel m)
        {
            var payload = new { Category_Name = m.Category_Name, Category_EntryAmount = m.Category_EntryAmount, Category_QualifyAge = m.Category_QualifyAge };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return (await _http.PutAsync($"api/categories/{m.Category_ID}", content)).IsSuccessStatusCode;
        }
        public async Task<bool> DeleteCategoryAsync(int id) => (await _http.DeleteAsync($"api/categories/{id}")).IsSuccessStatusCode;
        public async Task<List<RouteViewModel>> GetRoutesAsync(int eventId)
        {
            var res = await _http.GetAsync($"api/events/{eventId}/routes");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<RouteViewModel>>(await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        public async Task<bool> AddRouteAsync(RouteViewModel m)
        {
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(m.Route_Name), "Route_Name");
            form.Add(new StringContent(m.Route_Distance.ToString()), "Route_Distance");
            form.Add(new StringContent(m.Route_Map ?? ""), "Route_Map");
            if (m.MapImage != null)
            {
                var stream = new StreamContent(m.MapImage.OpenReadStream());
                stream.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(m.MapImage.ContentType);
                form.Add(stream, "mapImage", m.MapImage.FileName);
            }
            return (await _http.PostAsync($"api/events/{m.Event_ID}/routes", form)).IsSuccessStatusCode;
        }
        public async Task<bool> EnrolAsync(int eventId, int categoryId)
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            return (await _http.PostAsync($"/api/events/{eventId}/categories/{categoryId}/enrolments", content)).IsSuccessStatusCode;
        }
        public async Task<List<EnrolmentViewModel>> GetEnrolmentsForEventAsync(int eventId)
        {
            var res = await _http.GetAsync($"api/events/{eventId}/enrolments");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<EnrolmentViewModel>>(await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        public async Task<List<EnrolmentViewModel>> GetAllEnrolmentsAsync()
        {
            var res = await _http.GetAsync("api/enrolments");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<EnrolmentViewModel>>(await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        public async Task<List<EnrolmentViewModel>> GetMyEnrolmentsAsync(int participantId)
        {
            var all = await GetAllEnrolmentsAsync();
            return all.Where(e => e.Participant_ID == participantId).ToList();
        }
        public async Task<bool> DeleteEnrolmentAsync(int id) => (await _http.DeleteAsync($"api/enrolments/{id}")).IsSuccessStatusCode;
        public async Task<List<ResultViewModel>> GetResultsForEventAsync(int eventId)
        {
            var res = await _http.GetAsync($"api/events/{eventId}/results");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<ResultViewModel>>(await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        public async Task<bool> AddResultAsync(ResultViewModel m)
        {
            var payload = new { Finish_Time = m.Finish_Time, Finish_Position = m.Finish_Position, Average_Pace = m.Average_Pace, Category_Position = m.Category_Position };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return (await _http.PostAsync($"api/results/{m.Enrolment_ID}", content)).IsSuccessStatusCode;
        }
        public async Task<bool> UpdateResultAsync(ResultViewModel m)
        {
            var payload = new { Finish_Time = m.Finish_Time, Finish_Position = m.Finish_Position, Average_Pace = m.Average_Pace, Category_Position = m.Category_Position };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return (await _http.PutAsync($"api/results/{m.Results_ID}", content)).IsSuccessStatusCode;
        }
        public async Task<ProfileViewModel?> GetProfileAsync(int userId)
        {
            var res = await _http.GetAsync($"api/users/{userId}");
            if (!res.IsSuccessStatusCode) return null;
            var data = JsonSerializer.Deserialize<JsonElement>(await res.Content.ReadAsStringAsync(), _json);
            return new ProfileViewModel { User_ID = data.GetProperty("User_ID").GetInt32(), User_Email = data.GetProperty("User_Email").GetString()! };
        }
        public async Task<bool> UpdateProfileAsync(ProfileViewModel m)
        {
            var payload = new { User_Email = m.User_Email, User_Password = m.User_Password ?? "" };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return (await _http.PutAsync($"api/users/{m.User_ID}", content)).IsSuccessStatusCode;
        }
    }
}