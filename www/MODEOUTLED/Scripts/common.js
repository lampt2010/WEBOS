//banner 2 ben
function FloatTopDiv() {
    debugger;
    startLX = ((document.body.clientWidth - MainContentW) / 2) - LeftBannerW - LeftAdjust, startLY = TopAdjust + 140;
    startRX = ((document.body.clientWidth - MainContentW) / 2) + MainContentW + RightAdjust, startRY = TopAdjust + 140;
    var d = document;
    function ml(id) {
        debugger;
        var el = d.getElementById ? d.getElementById(id) : d.all ? d.all[id] : d.layers[id];
        el.sP = function (x, y) {
            this.style.left = x + 'px';
            this.style.top = y + 'px';
        };
        el.x = startRX;
        el.y = startRY;
        return el;
    }
    function m2(id) {
        debugger;
        var e2 = d.getElementById ? d.getElementById(id) : d.all ? d.all[id] : d.layers[id];
        e2.sP = function (x, y) {
            this.style.left = x + 'px';
            this.style.top = y + 'px';
        };
        e2.x = startLX;
        e2.y = startLY;
        return e2;
    }
    window.stayTopLeft = function () {
        if (document.documentElement && document.documentElement.scrollTop)
            var pY = document.documentElement.scrollTop;
        else if (document.body)
            var pY = document.body.scrollTop;
        if (document.body.scrollTop > 30) { startLY = 10; startRY = 10; } else { startLY = TopAdjust; startRY = TopAdjust; };
        ftlObj.y += (pY + startRY - ftlObj.y) / 16;
        ftlObj.sP(ftlObj.x, ftlObj.y);
        ftlObj2.y += (pY + startLY - ftlObj2.y) / 16;
        ftlObj2.sP(ftlObj2.x, ftlObj2.y);
        setTimeout("stayTopLeft()", 1);
    }
    ftlObj = ml("divAdRight");
    //stayTopLeft();     
    ftlObj2 = m2("divAdLeft");
    stayTopLeft();
}
function ShowAdDiv() {
    debugger;
    var objAdDivRight = document.getElementById("divAdRight");
    var objAdDivLeft = document.getElementById("divAdLeft");
    if (document.body.clientWidth < 1000) {
        debugger;
        objAdDivRight.style.display = "none";
        objAdDivLeft.style.display = "none";
    }
    else {
        debugger;
        objAdDivRight.style.display = "block";
        objAdDivLeft.style.display = "block";
        FloatTopDiv();
    }
}
function theRotator() {
    //Set the opacity of all images to 0
    $('div#rotator ul li').css({ opacity: 0.0 });

    //Get the first image and display it (gets set to full opacity)
    $('div#rotator ul li:first').css({ opacity: 1.0 });

    //Call the rotator function to run the slideshow, 6000 = change to next image after 6 seconds
    setInterval('rotate()', 9000);
}

function rotate() {
    //Get the first image
    var current = ($('div#rotator ul li.show') ? $('div#rotator ul li.show') : $('div#rotator ul li:first'));

    //Get next image, when it reaches the end, rotate it back to the first image
    var next = ((current.next().length) ? ((current.next().hasClass('show')) ? $('div#rotator ul li:first') : current.next()) : $('div#rotator ul li:first'));

    //Set the fade in effect for the next image, the show class has higher z-index
    next.css({ opacity: 0.0 })
.addClass('show')
.animate({ opacity: 1.0 }, 9500);

    //Hide the current image
    current.animate({ opacity: 0.0 }, 9000)
.removeClass('show');

};

function Slug(ele) {
    var title, slug;

    //Lấy text từ thẻ input title 
    title = $(ele).val();

    //Đổi chữ hoa thành chữ thường
    slug = title.toLowerCase();

    //Đổi ký tự có dấu thành không dấu
    slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
    slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
    slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
    slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
    slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
    slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
    slug = slug.replace(/đ/gi, 'd');
    //Xóa các ký tự đặt biệt
    slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
    //Đổi khoảng trắng thành ký tự gạch ngang
    slug = slug.replace(/ /gi, "-");
    //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
    //Phòng trường hợp người nhập vào quá nhiều ký tự trắng
    slug = slug.replace(/\-\-\-\-\-/gi, '-');
    slug = slug.replace(/\-\-\-\-/gi, '-');
    slug = slug.replace(/\-\-\-/gi, '-');
    slug = slug.replace(/\-\-/gi, '-');
    //Xóa các ký tự gạch ngang ở đầu và cuối
    slug = '@' + slug + '@';
    slug = slug.replace(/\@\-|\-\@|\@/gi, '');
    //In slug ra textbox có id “slug”
    return  slug;
}



$(document).ready(function () {
    //Load the slideshow
    theRotator();
});