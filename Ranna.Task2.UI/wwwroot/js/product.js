var selectedIds = [];  // seçilen ürünlerin ID'lerini tutacak dizi
var selectedProducts = []; // seçilen ürünlerin ad,kod,id ve fiyatlarini tutmak icin




//id degerine gore urun bilgilerini getiren fonksiyon
function getProductData(id) {
    return $.ajax({
        url: '/Product/GetProductById',
        type: 'GET',
        data: { id: id },
        dataType: 'json'
    });
}


//Yeni urun eklemek icin kullanilan fonksiyon
function addProduct(formData) {
    $.ajax({
        url: '/Product/AddProduct',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            Swal.fire('Başarılı!', 'Ürün başarıyla eklendi.', 'success');
            $('#productUpsertModal').modal('hide');
            $('#productTable').DataTable().ajax.reload();
        },
        error: function (xhr, status, error) {
            if (xhr.status === 400) {
                const hasErrorMessage = xhr.responseJSON.some(item => item.hasOwnProperty('propertyName'));
                if (hasErrorMessage) {
                    const validationErrors = xhr.responseJSON.filter(item => item.hasOwnProperty('propertyName'));
                    var message = "";
                    for (var i = 0; i < validationErrors.length; i++) {
                        message += `${validationErrors[i].errorMessage}<br>`;

                    }
                }
            }
            Swal.fire('Hata!', message, 'error');
        }
    });

}


//Ürün güncelleme işlemi için kullanılan fonksiyon
function updateProduct(formData) {
    $.ajax({
        url: '/Product/UpdateProduct',
        type: 'PUT',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            Swal.fire('Başarılı!', 'Ürün başarıyla güncellendi.', 'success');
            $('#productUpsertModal').modal('hide');
            $('#productTable').DataTable().ajax.reload();
            selectedIds = [];
            selectedProducts = [];
        },
        error: function (xhr, status, error) {
            if (xhr.status === 400) {
                const hasErrorMessage = xhr.responseJSON.some(item => item.hasOwnProperty('propertyName'));
                if (hasErrorMessage) {
                    const validationErrors = xhr.responseJSON.filter(item => item.hasOwnProperty('propertyName'));
                    var message = "";
                    for (var i = 0; i < validationErrors.length; i++) {
                        message += `${validationErrors[i].errorMessage}<br>`;

                    }
                }
            }
            Swal.fire('Hata!', message, 'error');
        }
    });
}



//ürün silme için kullanılan fonksiyon
function deleteProduct() {

    if (selectedIds.length === 0) {
        Swal.fire({
            icon: 'warning',
            title: 'Uyarı',
            text: 'En az 1 adet ürün seçmelisiniz',
        });
        return;
    }
    const productList = selectedProducts.map(p =>
        `<li>${p.kod} - ${p.ad} - ${p.fiyat}₺</li>`
    ).join('');

    Swal.fire({
        title: 'Emin misiniz?',
        html: `
            <p>Seçili ürünler silinecek:</p>
            <div style="max-height:200px; overflow-y:auto; text-align:left;">
                <ul style="padding-left:20px; margin:0;">${productList}</ul>
            </div>`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Evet, sil',
        cancelButtonText: 'İptal',
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: '/Product/DeleteProducts',
                method: 'DELETE',
                data: { ids: selectedIds },
                success: function (response) {
                    Swal.fire('Başarılı!', 'Seçilen ürünler silindi.', 'success');

                    $('#productTable').DataTable().ajax.reload();
                    selectedIds = [];
                    selectedProducts = [];
                },
                error: function () {
                    Swal.fire('Hata!', 'Silme işlemi başarısız oldu.', 'error');
                }
            });
        }
    });

}


//Modal basligini degistirmek icin
function changeModalTitle(type) {
    if (type === 'Add') {
        $("#productUpsertModalLabel").html("Ürün Ekle");
    } else if (type === 'Update') {
        $("#productUpsertModalLabel").html("Ürün Güncelle");
    }
}


// Modal acmak icin
function openUpsertModal(type) {
    changeModalTitle(type);
    if (type === 'Add') {
        $('#productUpsertModal').modal('show');
    }
    else if (type === 'Update') {

        if (selectedIds.length === 0) {
            Swal.fire({
                icon: 'warning',
                title: 'Uyarı',
                text: 'Ürün seçmeden güncelleme yapamazsınız',
            });
            return;
        }
        const lastSelectedProduct = selectedIds[selectedIds.length - 1];


        getProductData(lastSelectedProduct)
            .done(function (response) {


                $('#productId').val(response.id);
                $('#Kod').val(response.kod);
                $('#Ad').val(response.ad);
                $('#Fiyat').val(response.fiyat);
                $('#Bilgi').val(response.bilgi);
                $("#imagePreview").attr("src", response.resim);

                $('#productUpsertModal').modal('show');

            })
            .fail(function (xhr, status, error) {
                Swal.fire("Hata", "Ürün getirilemedi.Daha önceden silinmiş olabilir", "error");
            });




    }
}


// Urun ekleme veya guncelleme icin formdata hazirlayip gerekli fonksiyona gonder
function upsertProduct(event) {
    event.preventDefault();
    const id = $('#productId').val();
    const kod = $('#Kod').val();
    const ad = $('#Ad').val();
    const fiyat = $('#Fiyat').val();
    const bilgi = $('#Bilgi').val();
    const resim = $('#Resim')[0].files[0];
    const formData = new FormData();

    formData.append('Kod', kod);
    formData.append('Ad', ad);
    formData.append('Fiyat', fiyat);
    formData.append('Bilgi', bilgi);
    if (resim) {
        formData.append('Resim', resim);
    }
    else {
        formData.append('Resim', null);
    }
    if (id) {
        formData.append('Id', id);
        updateProduct(formData);
    }
    else {
        addProduct(formData);
    }
}