var unityWebView =
{
    loaded: [],

    init : function (name) {
        $containers = $('.webviewContainer');
        if ($containers.length === 0) {
            $('<div style="position: absolute; left: 0px; width: 100%; height: 100%; top: 0px; pointer-events: none;"><div class="webviewContainer" style="overflow: hidden; position: relative; width: 100%; height: 100%; z-index: 1;"></div></div>')
                .appendTo($('#gameContainer'));
        }
        var $last = $('.webviewContainer:last');
        var clonedTop = parseInt($last.css('top')) - 100;
        var $clone = $last.clone().insertAfter($last).css('top', clonedTop + '%');
        var $iframe =
            $('<iframe style="position:relative; width:100%; height:100%; border-style:none; display:none; pointer-events:auto;"></iframe>')
            .attr('id', 'webview_' + name)
            .appendTo($last)
            .on('load', function () {
                $(this).attr('loaded', 'true');
                var contents = $(this).contents();
                var w = $(this)[0].contentWindow;
                contents.find('a').click(function (e) {
                    var href = $.trim($(this).attr('href'));
                    if (href.substr(0, 6) === 'unity:') {
                        unityInstance.SendMessage(name, "CallFromJS", href.substring(6, href.length));
                        e.preventDefault();
                    } else {
                        w.location.replace(href);
                    }
                });

                contents.find('form').submit(function () {
                    $this = $(this);
                    var action = $.trim($this.attr('action'));
                    if (action.substr(0, 6) === 'unity:') {
                        var message = action.substring(6, action.length);
                        if ($this.attr('method').toLowerCase() == 'get') {
                            message += '?' + $this.serialize();
                        }
                        unityInstance.SendMessage(name, "CallFromJS", message);
                        return false;
                    }
                    return true;
                });

                unityInstance.SendMessage(name, "CallOnLoaded", location.href);
            });

            
        window.addEventListener('message', event => {
            const { data } = event;

            switch (data.message) {
                case 'BACK_TO_THE_GAME':
                case 'LOG_OUT':
                case 'STORE_IS_READY':
                case 'SCUTI_EXCHANGE':
                case 'USER_TOKEN':
                case 'NEW_REWARDS':
                case 'NEW_PRODUCTS':
                    this.onRecievedMessage(data);
                    
                    break;

                default:
                    break;
            }
        });
    },

    sendMessage: function (name, message) {
        unityInstance.SendMessage(name, "CallFromJS", message);
    },

    onRecievedMessage(message) {
        Unity.call(JSON.stringify(message));
    },

    setMargins: function (name, left, top, right, bottom) {
        var container = $('#gameContainer');
        var width = container.width();
        var height = container.height();

        var lp = left / width * 100;
        var tp = top / height * 100;
        var wp = (width - left - right) / width * 100;
        var hp = (height - top - bottom) / height * 100;

        this.iframe(name)
            .css('left', lp + '%')
            .css('top', tp + '%')
            .css('width', wp + '%')
            .css('height', hp + '%');
    },

    setVisibility: function (name, visible) {
        if (visible)
            this.iframe(name).show();
        else
            this.iframe(name).hide();
    },

    loadURL: function(name, url) {
        this.iframe(name).attr('loaded', 'false')[0].contentWindow.location.replace(url);
    },

    evaluateJS: function (name, js) {
        var prefix = "ld:";
        if (js.startsWith(prefix)) {
            var result = js.substring(prefix.length);
            eval(result);
        }else{
            this.sendMessageToIframe(name, js);
        }
    },

    sendMessageToIframe (name,message){
        var $iframelist = this.iframe(name);
        if($iframelist.length == 0){
            console.log("cannot find iframe");
            return;
        }
        $iframe = $iframelist[0];
        var targetOrigin = '*';
        
        if(message.includes("getNewProducts")){
            message = "{ message: 'GET_NEW_PRODUCTS' }";
        }else if (message.includes("getNewRewards")) {
            message = "{ message: 'GET_NEW_REWARDS' }";
        }else if(message.includes("toggleStore")){
            var messageObj = {
                message: 'TOGGLE_STORE',
                payload: true
              };
              message = messageObj;
        }else if(message.includes("setGameUserId")){
            var startIndex = message.indexOf('(') + 1;
            var endIndex = message.indexOf(')');
            var userID = message.substring(startIndex, endIndex);
            var messageObj = {
                message: 'SET_GAME_USER_ID',
                payload:  userID
                };
                message = messageObj;
            }
        if ($iframe.contentWindow) {
            $iframe.contentWindow.postMessage(message, targetOrigin);
        }else{
            $iframe.on('load', function(){
                $(this)[0].contentWindow.postMessage(message, targetOrigin);
            });
            $iframe.onload = function() {
                var iframeWindow = $iframe.contentWindow;
                if (iframeWindow) {
                    iframeWindow.postMessage(message, targetOrigin);
                } else {
                    console.error("iframe contentWindow is undefined.");
                }
            }

        }
    },

    destroy: function (name) {
        this.iframe(name).parent().parent().remove();
    },

    iframe: function (name) {
        return $('#webview_' + name);
    },    

};
