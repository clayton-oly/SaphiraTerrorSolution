//java script puro
//esta linha abaixo (evento) garante que o documento esta pronto (carregado)
$(document).ready(function () {

    //urlBase da API
    const urlBase = "https://localhost:7140/api"

    //consultando a API usando AJAX
    console.log('Chamando a API');

    $.ajax({
        url: urlBase + "/Genero", //Endpoint
        type: "GET",
        contentType: "application/json",
        //se der sucesso (200) cai aqui nesse bloco
        success: function (dados) {

            //selecionar a div dos generos
            const divGeneros = $('#genre');

            dados.forEach(genero => {

                //botoes de genero
                const btn = `<button class="btn btn-outline-light me-2 filter-btn" data-genero="${genero.idGenero}">${genero.descricaoGenero}</button>`;

                divGeneros.append(btn);
            });

            console.log('Deu sucesso!');
            console.log(dados);
            console.log(dados[1]);
            console.log(dados[1].descricaoGenero);
        },
        error: function (erro) {
            console.log('Deu erro!');
        }
    });
});
