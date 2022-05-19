document.addEventListener('DOMContentLoaded', function () {
    let eventsArr = [];

    let eventsTable = document.getElementById('eventsTable');

    let trElements = eventsTable.getElementsByTagName("tr");
    for (let tr of trElements) {
        let tdElements = tr.getElementsByTagName("td");
        let eventObj = {
            id: tdElements[0].innerText,
            title: tdElements[1].innerText,
            start: tdElements[2].innerText,
            end: tdElements[3].innerText,
            url: tdElements[4].innerText
        };

        eventsArr.push(eventObj);
    }

    var initialLocaleCode = 'vi';
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
        },
        locale: initialLocaleCode,
        buttonIcons: false, // show the prev/next text
        weekNumbers: true,
        navLinks: true, // can click day/week names to navigate views
        editable: true,
        dayMaxEvents: true, // allow "more" link when too many events
        events: eventsArr,
        eventClick: function (info) {
            info.jsEvent.preventDefault(); // don't let the browser navigate

            if (info.event.url) {
                window.open(info.event.url);
            }
        }
    });

    calendar.render();
    calendar.setOption('locale', 'vi');

});