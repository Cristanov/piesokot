$(window).load(function () {
    $('.flexslider').flexslider();

    var rater = $("#jRate").jRate({
        precision: 0.5,
        rating: parseFloat($('#average_company_rate').val().replace(',', '.')),
        //readOnly: isReadOnly(),
        onSet: function (rating) {

            var companyId = $("#company_id").val();
            var requestData = {
                companyId: companyId,
                rate: rating
            };

            //TODO: nie działa dla wielu obiektów
            var companyRateCookieName = 'companyRate' + companyId;
            var rateId = Cookies.get(companyRateCookieName);
            if (rateId != null) {

                //update rate
                console.log(rateId, ' - rate Id in update');
                requestData.rateId = rateId;
                $.getJSON('/Company/UpdateRate', requestData).done(function (data) {
                    // TODO: powiadomić jeśli błąd
                    rater.setRating(data.rating);
                });
            }
            else {

                $.getJSON("/Company/AddRate", requestData).done(function (data) {

                    // TODO: powiadomić jeśli błąd
                    rater.setRating(data.rating);
                    Cookies.set(companyRateCookieName, data.rateId);
                    console.log(companyRateCookieName, 'companyRateCookieName');
                });
            }
        }
    });

});