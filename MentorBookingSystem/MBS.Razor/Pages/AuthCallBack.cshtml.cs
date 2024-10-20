using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages;

public class AuthCallBack : PageModel
{
    public string State { get; set; }
    public string Code { get; set; }
    public string Scope { get; set; }

    public void OnGet(string state, string code, string scope)
    {
        // Lưu hoặc xử lý state, code, và scope ở đây
        State = state;
        Code = code;
        Scope = scope;

        // Sau đó bạn có thể dùng code để lấy token từ Google, ví dụ:
        // TokenResponse token = await GoogleAuthorizationCodeFlow.ExchangeCodeForTokenAsync(code);
    }
}