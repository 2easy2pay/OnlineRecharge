var Metronic = function () {
    var HandleCommon = function () {
        $('.toggle-menu').click(function () {
            $(this).parent().toggleClass('open');
            return false;
        });
        $('.user-icon').click(function () {
            $('.mobile-dropdown').slideUp();
            $(this).next('.user-account-dropdown').slideToggle();
            return false;
        });
        $('.search-icon').click(function () {
            $('.mobile-dropdown').slideUp();
            $(this).next('.search-form-wrap').slideToggle();
            return false;
        });
        $('.close-menu').click(function () {
            $('.mobile-header-menu').removeClass('open');
            $('.overlay').removeClass('visible');
            return false;
        });
        $('.close-menu-inner').click(function () {
            $('.sub-menu-ul').removeClass('open');
            return false;
        });
        $('.menu-title').click(function () {
            $('.mobile-header-menu').addClass('open');
            $('.overlay').addClass('visible');
            return false;
        });
        $('.sub-menu').click(function () {
            $(this).next('.sub-menu-ul').addClass('open');
            return false;
        });
        $('.overlay').click(function () {
            $('.mobile-header-menu').removeClass('open');
            $('.sub-menu-ul').removeClass('open');
            $('.overlay').removeClass('visible');
            return false;
        });
        if ($(window).width() < 767) {
            $('.footer-block .title').click(function () {
                $('.footer-block ul').slideUp();
                $(this).next('.list').slideToggle();
                return false;
            });
        };
    };
    return {
        init: function () {
            HandleCommon();
        }
    };
}();