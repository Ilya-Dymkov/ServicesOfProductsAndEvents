using Microsoft.AspNetCore.Mvc;

namespace ServicesOfProducts.Controllers.ControllersSource;

public static class ExtensionsForControllers
{
    public static async Task<ActionResult<TResult>> BaseActionGet<TResult>
        (this ControllerBase controller, Func<Task<TResult?>> func, string errorMessage)
    {
        var obj = await func();

        if (obj == null) throw new Exception(errorMessage);

        return controller.Ok(obj);
    }
}