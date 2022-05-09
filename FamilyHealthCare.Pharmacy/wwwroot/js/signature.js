function CanvasSignature(e) {
    var canvasId = e.getAttribute('data-canvas')
    var btnActionId = $('#' + canvasId).closest('form').find('button')[0].getAttribute('id');
    var actionsId = $('#' + canvasId).closest('form').find('button').next()[0].getAttribute('id');
    //console.log(actionsId);
    var canvas = document.querySelector('#' + canvasId);
    var ctx = canvas.getContext('2d');
    var buttonAction = document.getElementById(btnActionId);
    var actions = document.getElementById(actionsId);
    var imageBlob = document.getElementById('blobImg');

    var color = 'blue';
    var lineWidth = 2;
    //set default cursor pencil
    if (canvas.classList.contains('eraser-cursor')) {
        color = 'white';
        lineWidth = 15;
    }
    canvas.classList.add('pencil-cursor');
    // Resizing 
    canvas.height = 80;
    canvas.width = 215;

    //variables
    let painting = false;

    function startPosition(e) {
        painting = true;
        draw(e);
    }

    function finishedPosition() {
        painting = false;
        ctx.beginPath();
    }



    function draw(e) {
        if (!painting) return;
        ctx.lineWidth = lineWidth;
        ctx.lineCap = 'round';
        ctx.strokeStyle = color;
        var offset = canvas.getBoundingClientRect();

        ctx.lineTo(e.pageX - offset.left, e.pageY - offset.top);
        ctx.stroke();
        ctx.beginPath();
        ctx.moveTo(e.pageX - offset.left, e.pageY - offset.top);
    }


    //EventListeners
    canvas.addEventListener('mousedown', startPosition);
    canvas.addEventListener('mouseup', finishedPosition);
    canvas.addEventListener('mousemove', draw);
    canvas.addEventListener('mouseout', finishedPosition);

    buttonAction.addEventListener('click', function () {
        var actionElements = actions.children;
        for (let i = 0; i < actionElements.length; i++) {
            actionElements[i].addEventListener('click', function () {
                var changeColor = actionElements[i].getAttribute('id');
                switch (changeColor) {
                    case "blue":
                        color = 'blue';
                        canvas.classList.remove('eraser-cursor');
                        canvas.classList.add('pencil-cursor');
                        break;
                    case "white":
                        color = 'white';
                        canvas.classList.remove('pencil-cursor');
                        canvas.classList.add('eraser-cursor');
                        break;
                }
                if (color == 'white') lineWidth = 15;
                else lineWidth = 2;
                var imgElement = actionElements[i].children[0];
                buttonAction.children[0].setAttribute('src', imgElement.getAttribute('src'));
            })
        }
    })
}

// function changeColor(obj) {
//     switch (obj.id) {
//         case "blue":
//             color = 'blue';
//             canvas.classList.remove('eraser-cursor');
//             canvas.classList.add('pencil-cursor');
//             break;
//         case "white":
//             color = '#fff';
//             canvas.classList.remove('pencil-cursor');
//             canvas.classList.add('eraser-cursor');
//             break;
//     }
//     if (color == '#fff') lineWidth = 15;
//     else lineWidth = 2;
// }



function erase(e) {
    var canvas = $('#' + e.id).closest('.signature-wrap').find('canvas')[0];
    var ctx = canvas.getContext('2d');
    var m = confirm("Want to clear");
    if (m) {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        //document.getElementById("canvasimg").style.display = "none";
    }
}

function save() {
    document.getElementById("canvasimg").style.border = "2px solid";
    var dataURL = canvas.toDataURL();
    document.getElementById("canvasimg").src = dataURL;
    document.getElementById("canvasimg").style.display = "block";
    var blob = dataURItoBlob(dataURL);
    var blobUrl = URL.createObjectURL(blob);
    var file = new File([blob], "signature", { type: blob.type });
    console.log(file);
    imageBlob.src = blobUrl;
}

function dataURItoBlob(dataURI) {
    // convert base64 to raw binary data held in a string
    // doesn't handle URLEncoded DataURIs - see SO answer #6850276 for code that does this
    var byteString = atob(dataURI.split(',')[1]);

    // separate out the mime component
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]

    // write the bytes of the string to an ArrayBuffer
    var ab = new ArrayBuffer(byteString.length);

    // create a view into the buffer
    var ia = new Uint8Array(ab);

    // set the bytes of the buffer to the correct values
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    // write the ArrayBuffer to a blob, and you're done
    var blob = new Blob([ab], { type: mimeString });
    return blob;
}