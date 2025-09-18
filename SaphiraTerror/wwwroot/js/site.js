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

    //Ao clicar em um dos generos (elemento button dentro da div id genre)
    $(document).on('click', '#genre button', function () {
        //pegar o valor do atributo 'data-genero do botão clicado
        const generoEscolhido = $(this).data('genero');
        console.log('Gênero:', generoEscolhido);

        //chamar a função buscarFilmes, passando o genero
        buscarFilmes(generoEscolhido);

    });

    buscarFilmes("todos");

    function buscarFilmes(generoEscolhido) {
        url = "/filme";
        if (generoEscolhido != 'todos') {
            url = url + "/" + generoEscolhido;
        }
        $.ajax({
            url: urlBase + url,
            type: "GET",
            contentType: "application/json",
            //se der sucesso (200) cai aqui nesse bloco
            success: function (dados) {

                //selecionar a div dos filmes
                const divFilmes = $('#filmes-container');
                divFilmes.html("");

                dados.forEach(filme => {

                    //cor do selo de classificação
                    corBagde = "success";
                    if (filme.classificacao == "+18") {
                        corBagde = "danger";
                    } else if (filme.classificacao == "Kids") {
                        corBagde = "primary";
                    }

                    const card = `<div class="col">
                    <div class="card bg-dark border-secondary h-100">
                        <img src="${filme.urlImagem}" class="card-img-top" alt="${filme.titulo}" style="height: 250px; object-fit: cover;">
                        <div class="card-body">
                            <h5 class="card-title">${filme.titulo}</h5>
                            <p class="card-text text-muted">${filme.genero}</p>
                            <p class="card-text small text-muted">${filme.produtora}</p>
                            <span class="badge bg-${corBagde}">${filme.classificacao}</span>
                        </div>
                    </div>
                </div>`;

                    divFilmes.append(card);
                });
            },
            error: function (erro) {
                console.log('Erro ao carrregar filmes!');
            }
        });
    }

    //Busca por título. Quando teclar algo na caixa de busca
    $('#busca-filme').on('keyup', function () {

        // Pega o valor do campo de busca e converte para minúsculas
        const termoBusca = $(this).val().toLowerCase();

        // Percorrer cada div de filme na div 'filmes-container'
        $('#filmes-container div').each(function () {
            // Pega o texto do título do filme
            const tituloFilme = $(this).find('.card-title').text().toLowerCase();

            // Verifica se o título do filme inclui o termo de busca
            if (tituloFilme.includes(termoBusca)) {
                // Se sim, mostra o card
                $(this).show();
            } else {
                // Se não, oculta o card
                $(this).hide();
            }
        });
    });
});
