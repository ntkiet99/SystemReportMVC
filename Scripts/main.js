(function () {
    window.Loading = false;

    function disableScrolling() {
        var x = window.scrollX;
        var y = window.scrollY;
        window.onscroll = function () { window.scrollTo(x, y); };
    }

    function enableScrolling() {
        window.onscroll = function () { };
    }

    window.AddLoading = function AddLoading() {
        $('body').append('<div style="z-index: 999999" id="loadingDiv"><div class="loader">Loading...</div></div>');
        disableScrolling()
        window.Loading = true;
    }

    window.removeLoader = function removeLoader() {
        $("#loadingDiv").remove();
        enableScrolling();
        window.Loading = false;
    }

    toastr.options = {
        'closeButton': true,
        'debug': false,
        'newestOnTop': true,
        'progressBar': true,
        'positionClass': 'toast-top-right',
        'preventDuplicates': false,
        'showDuration': '1000',
        'hideDuration': '1000',
        'timeOut': '1000',
        'extendedTimeOut': '1000',
        'showEasing': 'swing',
        'hideEasing': 'linear',
        'showMethod': 'fadeIn',
        'hideMethod': 'fadeOut',
        "preventOpenDuplicates": false,
        'maxOpened': 1,
    }


})();