// Example 1:
//     The Book class violates SRP because it handles:
//         Book details: getTitle() and getAuthor()
//         State: turnPage()
//         Location: getLocation()
//         Persistence: save()

//     Because of this if one responsibility changes it may need to modify the entire class
//     and this also creates tight coupling between changes, which is not follow in SRP.

//     To follow SRP separate these into individual classes:
//         BookDetails: title and author
//         BookState: page turning
//         BookLocation: books location
//         BookSaver: saving

class Book {
    function getTitle() {
        return "A Great Book";
    }

    function getAuthor() {
        return "John Doe";
    }
}

class Reader implements Book{
    function nextPage() {
        // pointer to next page
    }

    function getCurrentPageContent() {
        return "current page content";
    }
}

class LibraryManager implements Book{
    function getLocation() {
        // returns the position in the library
        // ie. shelf number & room number
    }

    function save(Book $book) {
        $filename = '/documents/'. $this->getTitle(). ' - ' . $this->getAuthor();
        filePutContents($filename, serialize($this));
    }
}


------------------------------------------------------------------------------------------------------------------------

// Example 2:

// The Printer interface and its classes follow SRP because each class has one responsibility:
//     Printer interface: Responsible for print a page.
//     PlainTextPrinter: Responsible for printing pages in plain text.
//     HtmlPrinter: Responsible for printing pages in HTML format.

// Each class has one clear responsibility. If printing methods change, only the respective class changes


interface Printer {
    function printPage(string $pageContent);
}

class PlainTextPrinter implements Printer {
    function printPage(string $pageContent) {
        echo $pageContent;
    }
}

class HtmlPrinter implements Printer {
    function printPage(string $pageContent) {
        echo '<div style="single-page">' . $pageContent . '</div>';
    }
}
