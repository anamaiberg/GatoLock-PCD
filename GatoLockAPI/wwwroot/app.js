const gatos = [
  {
    id: 1,
    nome: "Virgil",
    idade: "2 anos",
    descricao: "Muito carinhoso e dorminhoco.",
    imagem:
      "assets/imagens/virgil.jpg"
  },

  {
    id: 2,
    nome: "Nyx",
    idade: "1 ano",
    descricao: "Brincalhona e cheia de energia.",
    imagem:
      "assets/imagens/nyx.jpg"
  },

  {
    id: 3,
    nome: "De Selby",
    idade: "3 anos",
    descricao: "Calmo, observador e amoroso.",
    imagem:
      "assets/imagens/deSelby.jpg"
  }
];

const listaGatos = document.getElementById("lista-gatos");

function renderizarGatos() {

  gatos.forEach((gato) => {

    listaGatos.innerHTML += `
    
      <div class="col-lg-4 mb-4">

        <div class="card-gato">

          <img src="${gato.imagem}" alt="${gato.nome}" />

          <div class="card-body-custom">

            <h3 class="gato-nome">
              ${gato.nome}
            </h3>

            <p class="gato-info">
              ${gato.idade}
            </p>

            <p>
              ${gato.descricao}
            </p>

            <button
              class="btn btn-pink w-100"
              data-bs-toggle="modal"
              data-bs-target="#modalAdocao"
            >
              🐾 Entrar na fila
            </button>

          </div>

        </div>

      </div>
    
    `;
  });

}

renderizarGatos();
