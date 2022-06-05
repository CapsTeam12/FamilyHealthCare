$(document).ready(function () {
    var trElements = $('.datatable').DataTable().rows().nodes();
    for (var i = 0; i < trElements.length; i++) {
        let checkboxEl = trElements[i].querySelector("[id^=status_]");
        $(checkboxEl).click(function () {
            var accountId = checkboxEl.getAttribute('data-accountId');
            var IsActive = checkboxEl.getAttribute('data-IsActive');
            var actionName = (!this.checked) ? "deactivate" : "activate";
            var checked = $(this).is(':checked');
            swal({
                title: `Do you want to ${actionName} this account?`,
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((confirm) => {
                    if (confirm) {
                        ChangeStatusAccount(accountId, IsActive, checkboxEl, actionName);
                    } else if (!confirm && !checked) {
                        $(this).prop('checked', !checked);
                    } else if (!confirm && checked) {
                        $(this).prop('checked', !checked);
                    }
                });
        })
    }
})

function ChangeStatusAccount(AccountId, Status, Element, ActionName) {
    $.ajax({
        url: '/Patient/ActiveAndDeActivateAccount',
        type: 'PUT',
        data: { accountId: AccountId, isActive: Status },
        success: function (res) {
            if (res) {
                Element.setAttribute('data-IsActive', res.isActive);
                swal(`Success! This account has been ${ActionName}!`, {
                    icon: "success",
                });
            }
        }
    });
}