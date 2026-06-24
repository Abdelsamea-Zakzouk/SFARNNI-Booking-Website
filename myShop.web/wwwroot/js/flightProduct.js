var dtble;
$(document).ready(function () {
  loaddata();
});

function loaddata() {
  dtble = $("#flightTable").DataTable({
    ajax: {
      url: "/Admin/FlightProduct/GetData", // Adjust this URL to match your actual endpoint for fetching flight products
    },
    columns: [
      { data: "name" }, // Corresponds to the 'Name' property in FlightProduct
      { data: "description" }, // Corresponds to the 'Description' property
      { data: "ticketPrice" }, // Corresponds to the 'TicketPrice' property
      { data: "locationFrom" }, // Corresponds to the 'LocationFrom' property
      { data: "locationTo" }, // Corresponds to the 'LocationTo' property
      { data: "category.name" }, // Ensure your FlightProduct has a navigation property to Category
      {
        data: "id",
        render: function (data) {
          return `
                        <a href="/Admin/FlightProduct/Edit/${data}" class="btn btn-success">Edit</a>
                        <a onClick=DeleteItem("/Admin/FlightProduct/Delete/${data}") class="btn btn-danger">Delete</a>
                    `;
        },
      },
    ],
  });
}

function DeleteItem(url) {
  Swal.fire({
    title: "Are you sure?",
    text: "You won't be able to revert this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Yes, delete it!",
  }).then((result) => {
    if (result.isConfirmed) {
      $.ajax({
        type: "DELETE",
        url: url,
        success: function (data) {
          if (data.success) {
            toastr.success(data.message);
            dtble.ajax.reload();
          } else {
            toastr.error(data.message);
          }
        },
      });
    }
  });
}
