$(document).ready(function () {
    $('.devmastermenu-list > li').click(function () {
        if ($(this).hasClass('active')) {
            $('.devmastermenu-list > li').removeClass('active');
        }
        else {
            $('.devmastermenu-list > li').removeClass('active');
            $(this).addClass('active');
        }
    });
    $('.mylist2  li').click(function () {
        if ($(this).hasClass('active')) {
            $('.mylist2 li').removeClass('active');
        }
        else {
            $('.mylist2 li').removeClass('active');
            $(this).addClass('active');
        }
    });
    $('.mylist2 > li > ul > li').click(function () {
        if ($(this).hasClass('active')) {
            $('.mylist2 > li > ul > li').removeClass('active');
        }
        else {
            $('.mylist2 > li > ul > li').removeClass('active');
            $(this).addClass('active');
        }
    });
    $('.search-mn button i').click(function () {
        $('.devmastermenu-list').slideUp();
        $('.search-mn button').slideUp();
        $('.search-mn').slideDown();        
        $('.inp-search').slideDown();
        
    });
    $('.out-search').click(function () {
        $('.inp-search').slideUp();
        $('.search-mn button').slideDown();
        $('.devmastermenu-list').slideDown();
         
    }); 
    //$('.search-mn button i').click(function () {
    //    $('.search-mn').slideUp();
    //    $('.devmastermenu-list').slideUp();
    //    $('.inp-search').slideDown();
    //});
    //$('.out-search').click(function () {
    //    $('this').slideUp();
    //    $('.devmastermenu-list').slideDown();
    //    $('.search-mn').slideDown();
    //});    
});