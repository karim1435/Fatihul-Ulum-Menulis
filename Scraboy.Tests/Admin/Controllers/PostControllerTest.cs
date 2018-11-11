using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScraBoy.Areas.Admin.Controllers;
using Telerik.JustMock;
using ScraBoy.Data;
using System.Web.Mvc;
using ScraBoy.Features.Blog.Pos;

namespace Scraboy.Tests.Admin.Controllers
{
    [TestClass]
    public class PostControllerTest
    {
        [TestMethod]
        public void Edit_GetRequestSendToView()
        {
            var id = "test-post";

            var repo = Mock.Create<IPostRepository>();
            var controller = new PostController(repo);

            Mock.Arrange(() => repo.Get(id)).
                Returns(new PostModel { Id = id });

            var result = (ViewResult)controller.Edit(id);

            var model = (PostModel)result.Model;

            Assert.AreEqual(id,model.Id);
        }

        [TestMethod]
        public void Edit_GetRequestNotFoundResult()
        {
            var id = "test-post";

            var repo = Mock.Create<IPostRepository>();
            var controller = new PostController(repo);

            Mock.Arrange(() => repo.Get(id)).
                Returns((PostModel)null);

            var result = controller.Edit(id);

            Assert.IsTrue(result is HttpNotFoundResult);
        }

        [TestMethod]
        public void Edit_PostRequestNotFoundResult()
        {
            var id = "test-post";

            var repo = Mock.Create<IPostRepository>();
            var controller = new PostController(repo);

            Mock.Arrange(() => repo.Get(id)).
                Returns((PostModel)null);

            var result = controller.Edit(id,new PostModel());

            Assert.IsTrue(result is HttpNotFoundResult);
        }

        [TestMethod]
        public void Edit_PostRequestSendPostToView()
        {
            var id = "test-post";

            var repo = Mock.Create<IPostRepository>();
            var controller = new PostController(repo);

            Mock.Arrange(() => repo.Get(id)).
                Returns(new PostModel { Id = id });

            controller.ViewData.ModelState.AddModelError("key","error message");

            var result = (ViewResult)controller.Edit(id, new PostModel() { Id = "test-post-2" });
            var model = (PostModel)result.Model;

            Assert.AreEqual("test-post-2",model.Id);
        }


        [TestMethod]
        public void Edit_PostRequestCalsEditAndRedirects()
        {
            var repo = Mock.Create<IPostRepository>();
            var controller = new PostController(repo);

            Mock.Arrange(() => repo.Edit(Arg.IsAny<string>(),Arg.IsAny<PostModel>())).
                MustBeCalled();

            var result =controller.Edit("foo",new PostModel() { Id = "test-post-2" });

            Mock.Assert(repo);

            Assert.IsTrue(result is RedirectToRouteResult);
        }
    }
}
