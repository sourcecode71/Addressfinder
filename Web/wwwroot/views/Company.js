﻿
  const app = new Vue({
      el: '#addCompany',
      beforeMount() {
          this.loadAllClients();
          this.loadAllCompany();
          
    },
    data: {
            errors: [],
            client: '0',
            name: null,
            contactName: '',
            email: null,
            phone:'',
            address: null,
            seen: false,
            clients: [],
            companies:[]

        },
      methods: {
       
      submitForm: function (e) {

         this.errors = [];

           
       if(this.isValidFrom())
       {

        const config = {headers: {'Content-Type': 'application/json' } };
        var base_url = window.location.origin;

           const clientURL = base_url + "/api/Company/save-company";

           const clientData =
           {
            clientId: this.client.id,
            name : this.name,
            address : this.address,
            contactName:this.contactName,
            email : this.email,
            phone : this.phone
           };

         axios.post(clientURL, clientData, config)
                .then(response =>{
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Record has been added successfully!',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    this.clearAll();
                }).catch(errors => {
                    Swal.fire({
                        position: 'top-end',
                        title: 'Error!',
                        text: 'Something went wrong.' + errorThrown.errorMessage,
                        icon: 'error',
                        confirmButtonText: 'Ok',
                    })
               });
          }
       },
        isValidFrom: function () {
            this.errors = [];

        if (!this.client || this.client==0) {
            this.errors.push("Name is required.");
        }

        if (!this.name) {
             this.errors.push("Name is required.");
        }

        if (!this.contactName) {
            this.errors.push("Contact name is required.");
            }


        if (!this.phone) {
          this.errors.push("Phone number is required.");
        }

        if (!this.email) {
        this.errors.push('Email is required.');
        } else if (!this.validateEmail(this.email)) {
        this.errors.push('Valid email is required.');
        }

            if (!this.errors.length)
            {
                  return true;
            }
     },
        
        handleUserInput: function (e)
        {
            var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
            this.phone = '(' + x[1] + ') ' + x[2] + '-' + x[3];
        },
        clearAll: function ()
        {
            this.client ='0';
            this.name = "";
            this.email = "";
            this.phone ="";
            this.address ="";
            this.contactName ="";
        },
        validateEmail(email) {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
                return true;
            } else {
                return false;
            }
          },
        loadAllClients: function () {

                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/company/all-clients";

                axios.get(clientURL, config).then(result => {

                    console.log(result.data);
                    this.clients = result.data;

                    setTimeout(() => {
                        $('#allClients').DataTable({
                            "scrollY": "500px",
                            "scrollCollapse": true,
                            "paging": false
                        });
                    }, 100);

               
                
                }, error => {
                    console.error(error);
                });

          },
          loadAllCompany: function () {

              const config = { headers: { 'Content-Type': 'application/json' } };
              var base_url = window.location.origin;
              const clientURL = base_url + "/api/company/all-companies";

              axios.get(clientURL, config).then(result => {

                  console.log(result.data);
                  this.companies = result.data;

                  setTimeout(() => {
                      $('#allCompanies').DataTable({
                          "scrollY": "500px",
                          "scrollCollapse": true,
                          "paging": false
                      });
                  }, 100); 



              }, error => {
                  console.error(error);
              });

          },
        showCompanyByClient: function (client) {

            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/company/all-companies-client?guid=" + client.id;

            $("#allCompany").modal("show");

            axios.get(clientURL, config).then(result => {

                console.log(result.data);
                this.companies = result.data;
            }, error => {
                Swal.fire({
                    position: 'top-end',
                    title: 'Error!',
                    text: 'Something went wrong.' + error,
                    icon: 'error',
                    confirmButtonText: 'Ok',
                })
            });

          }

      }
  })


   
                    
