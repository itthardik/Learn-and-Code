var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g = Object.create((typeof Iterator === "function" ? Iterator : Object).prototype);
    return g.next = verb(0), g["throw"] = verb(1), g["return"] = verb(2), typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
// Add an event listener to handle form submission when the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function () {
    var tumblrForm = document.getElementById("tumblrForm");
    if (tumblrForm) {
        tumblrForm.addEventListener("submit", handleFormSubmit);
    }
});
function handleFormSubmit(event) {
    event.preventDefault();
    var blogNameInput = document.getElementById("blogName");
    var postRangeInput = document.getElementById("postRange");
    var outputDiv = document.getElementById("output");
    if (!blogNameInput || !postRangeInput || !outputDiv) {
        console.error("Required elements are missing in the DOM.");
        return;
    }
    var blogName = blogNameInput.value;
    var postRange = postRangeInput.value;
    outputDiv.innerHTML = "";
    if (!isValidBlogInput(blogName, postRange)) {
        outputDiv.innerHTML = "<p class=\"text-red-500 text-center\">Please enter a valid blog name and post range (e.g., 1-5).</p>";
        return;
    }
    var _a = postRange.split("-").map(Number), start = _a[0], end = _a[1];
    if (start > end) {
        outputDiv.innerHTML = "<p class=\"text-red-500 text-center\">Start value cannot be greater than end value.</p>";
        return;
    }
    fetchTumblrData(blogName, start, end, outputDiv);
}
function isValidBlogInput(blogName, postRange) {
    //Matches a range of numbers in the format "start-end" (e.g., "10-20").
    var postRangePattern = /^\d+-\d+$/;
    return blogName && postRangePattern.test(postRange);
}
// Fetch photo posts from a Tumblr blog and display them in the given range.
function fetchTumblrData(blogName, start, end, outputDiv) {
    return __awaiter(this, void 0, void 0, function () {
        var apiUrl, response, text, data, _a;
        return __generator(this, function (_b) {
            switch (_b.label) {
                case 0:
                    _b.trys.push([0, 3, , 4]);
                    apiUrl = "https://".concat(blogName, ".tumblr.com/api/read/json?type=photo&num=").concat(end - start + 1, "&start=").concat(start - 1);
                    return [4 /*yield*/, fetch(apiUrl)];
                case 1:
                    response = _b.sent();
                    return [4 /*yield*/, response.text()];
                case 2:
                    text = _b.sent();
                    data = void 0;
                    try {
                        data = JSON.parse(text.replace(/^var tumblr_api_read = |;$/g, ""));
                    }
                    catch (error) {
                        outputDiv.innerHTML = "<p class=\"text-red-500 text-center\">Error parsing data. The response might be malformed or empty.</p>";
                        return [2 /*return*/];
                    }
                    outputDiv.innerHTML = getBlogInfo(data);
                    displayImages(data.posts, start, outputDiv);
                    return [3 /*break*/, 4];
                case 3:
                    _a = _b.sent();
                    outputDiv.innerHTML = "<p class=\"text-red-500 text-center\">Error fetching data. Please check the blog name.</p>";
                    return [3 /*break*/, 4];
                case 4: return [2 /*return*/];
            }
        });
    });
}
function getBlogInfo(data) {
    return "\n        <h3 class=\"text-xl font-semibold\">Blog Info</h3>\n        <p><strong>Title:</strong> ".concat(data.tumblelog.title, "</p>\n        <p><strong>Name:</strong> ").concat(data.tumblelog.name, "</p>\n        <p><strong>Description:</strong> ").concat(data.tumblelog.description, "</p>\n        <p><strong>Total Posts:</strong> ").concat(data.posts.length, "</p>\n        <h3 class=\"text-xl font-semibold mt-4\">Images</h3>\n    ");
}
function displayImages(posts, start, outputDiv) {
    posts.forEach(function (post, index) {
        if (post["photo-url-1280"]) {
            outputDiv.innerHTML += "<p class=\"font-bold mt-2\">Post ".concat(start + index, ":</p><img src=\"").concat(post["photo-url-1280"], "\" alt=\"Tumblr Image\" class=\"w-full rounded mt-2\">");
        }
    });
}
