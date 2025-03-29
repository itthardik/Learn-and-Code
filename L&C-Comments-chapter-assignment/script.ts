// Add an event listener to handle form submission when the DOM is fully loaded
document.addEventListener("DOMContentLoaded", () => {
    const tumblrForm = document.getElementById("tumblrForm");
    if (tumblrForm) {
        tumblrForm.addEventListener("submit", handleFormSubmit);
    }
});

function handleFormSubmit(event: { preventDefault: () => void; }) {
    event.preventDefault();
    const blogNameInput = document.getElementById("blogName") as HTMLInputElement | null;
    const postRangeInput = document.getElementById("postRange") as HTMLInputElement | null;
    const outputDiv = document.getElementById("output") as HTMLDivElement | null;

    if (!blogNameInput || !postRangeInput || !outputDiv) {
        console.error("Required elements are missing in the DOM.");
        return;
    }

    const blogName = blogNameInput.value;
    const postRange = postRangeInput.value;
    outputDiv.innerHTML = "";

    if (!isValidBlogInput(blogName, postRange)) {
        outputDiv.innerHTML = `<p class="text-red-500 text-center">Please enter a valid blog name and post range (e.g., 1-5).</p>`;
        return;
    }
    var [start, end] = postRange.split("-").map(Number);
    if (start > end) {
        outputDiv.innerHTML = `<p class="text-red-500 text-center">Start value cannot be greater than end value.</p>`;
        return;
    }
    fetchTumblrData(blogName, start, end, outputDiv);
}

function isValidBlogInput(blogName: string, postRange: string) {  
    //Matches a range of numbers in the format "start-end" (e.g., "10-20").
    const postRangePattern = /^\d+-\d+$/
    return blogName && postRangePattern.test(postRange);
}

// Fetch photo posts from a Tumblr blog and display them in the given range.
async function fetchTumblrData(blogName: string, start: number, end: number, outputDiv: HTMLDivElement) {
    try {
        const apiUrl = `https://${blogName}.tumblr.com/api/read/json?type=photo&num=${end - start + 1}&start=${start - 1}`;
        const response = await fetch(apiUrl);
        const text = await response.text();
        let data: { posts: any[]; };
        try {
            data = JSON.parse(text.replace(/^var tumblr_api_read = |;$/g, ""));
        } catch (error) {
            outputDiv.innerHTML = `<p class="text-red-500 text-center">Error parsing data. The response might be malformed or empty.</p>`;
            return;
        }

        outputDiv.innerHTML = getBlogInfo(data);
        displayImages(data.posts, start, outputDiv);
    } catch {
        outputDiv.innerHTML = `<p class="text-red-500 text-center">Error fetching data. Please check the blog name.</p>`;
    }
}

function getBlogInfo(data: any) {
    return `
        <h3 class="text-xl font-semibold">Blog Info</h3>
        <p><strong>Title:</strong> ${data.tumblelog.title}</p>
        <p><strong>Name:</strong> ${data.tumblelog.name}</p>
        <p><strong>Description:</strong> ${data.tumblelog.description}</p>
        <p><strong>Total Posts:</strong> ${data.posts.length}</p>
        <h3 class="text-xl font-semibold mt-4">Images</h3>
    `;
}

function displayImages(posts: any[], start: number, outputDiv: HTMLDivElement) {
    posts.forEach((post, index) => {
        if (post["photo-url-1280"]) {
            outputDiv.innerHTML += `<p class="font-bold mt-2">Post ${start + index}:</p><img src="${post["photo-url-1280"]}" alt="Tumblr Image" class="w-full rounded mt-2">`;
        }
    });
}
