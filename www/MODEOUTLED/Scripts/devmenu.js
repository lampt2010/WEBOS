var _0x68gt = [".overlapblackbg, .slideLeft", ".devmastermenucontent", "menuopen", "addClass", "menuclose", "removeClass", "hasClass", "click", "#navToggle", "mrginleft", "toggleClass", ".devmastermenucontainer", "on", "#navToggle,.overlapblackbg", '<span class="devmastermenu-click"><i class="devmastermenu-arrow fa fa-angle-down"></i></span>', "prepend", ".devmastermenu-submenu, .devmastermenu-submenu-sub, .devmastermenu-submenu-sub-sub", "has", ".devmastermenu-list li", ".megamenu", "slow", "slideToggle", ".devmastermenu-list", ".devmastermenu-mobile", ".devmastermenu-submenu",
    "siblings", "devmastermenu-rotate", ".devmastermenu-arrow", "children", ".devmastermenu-submenu-sub", ".devmastermenu-submenu-sub-sub", ".devmastermenu-click"
];
$(function() {
    var headings = $(_0x68gt[0]);
    var emptyJ = $(_0x68gt[1]);
    /**
     * @return {undefined}
     */
    var backdrop = function() {
        $(headings)[_0x68gt[5]](_0x68gt[4])[_0x68gt[3]](_0x68gt[2]);
    };
    /**
     * @return {undefined}
     */
    var _element = function() {
        $(headings)[_0x68gt[5]](_0x68gt[2])[_0x68gt[3]](_0x68gt[4]);
    };
    $(_0x68gt[8])[_0x68gt[7]](function() {
        if (emptyJ[_0x68gt[6]](_0x68gt[2])) {
            $(_element);
        } else {
            $(backdrop);
        }
    });
    emptyJ[_0x68gt[7]](function() {
        if (emptyJ[_0x68gt[6]](_0x68gt[2])) {
            $(_element);
        }
    });
    $(_0x68gt[13])[_0x68gt[12]](_0x68gt[7], function() {
        $(_0x68gt[11])[_0x68gt[10]](_0x68gt[9]);
    });
    $(_0x68gt[18])[_0x68gt[17]](_0x68gt[16])[_0x68gt[15]](_0x68gt[14]);
    $(_0x68gt[18])[_0x68gt[17]](_0x68gt[19])[_0x68gt[15]](_0x68gt[14]);
    $(_0x68gt[23])[_0x68gt[7]](function() {
        $(_0x68gt[22])[_0x68gt[21]](_0x68gt[20]);
    });
    $(_0x68gt[31])[_0x68gt[7]](function() {
        $(this)[_0x68gt[25]](_0x68gt[24])[_0x68gt[21]](_0x68gt[20]);
        $(this)[_0x68gt[28]](_0x68gt[27])[_0x68gt[10]](_0x68gt[26]);
        $(this)[_0x68gt[25]](_0x68gt[29])[_0x68gt[21]](_0x68gt[20]);
        $(this)[_0x68gt[25]](_0x68gt[30])[_0x68gt[21]](_0x68gt[20]);
        $(this)[_0x68gt[25]](_0x68gt[19])[_0x68gt[21]](_0x68gt[20]);
    });
});