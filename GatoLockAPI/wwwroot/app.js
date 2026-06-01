const gatos = [
  {
    id: 1,
    nome: "Virgil",
    idade: "2 anos",
    descricao: "Muito carinhoso e dorminhoco.",
    imagem: "assets/imagens/virgil.jpg"
  },
  {
    id: 2,
    nome: "Nyx",
    idade: "1 ano",
    descricao: "Brincalhona e cheia de energia.",
    imagem: "assets/imagens/nyx.jpg"
  },
  {
    id: 3,
    nome: "De Selby",
    idade: "3 anos",
    descricao: "Calmo, observador e amoroso.",
    imagem: "assets/imagens/deSelby.jpg"
  }
];

const listaGatos =
  document.getElementById("lista-gatos");

let gatoSelecionado = "";

function renderizarGatos() {

  listaGatos.innerHTML = "";

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
              class="btn btn-pink w-100 btn-adotar"
              data-gato="${gato.nome}"
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

  registrarBotoes();
}

function registrarBotoes() {

  document
    .querySelectorAll(".btn-adotar")
    .forEach(botao => {

      botao.addEventListener(
        "click",
        () => {

          gatoSelecionado =
            botao.dataset.gato;

          document
            .getElementById("nome-gato")
            .value =
            gatoSelecionado;
        }
      );
    });
}

const form =
  document.getElementById(
    "form-adocao"
  );

form.addEventListener(
  "submit",
  async (e) => {

    e.preventDefault();

    const dados = {

      nomeAdotante:
        document.getElementById(
          "nome-adotante"
        ).value,

      nomeGato:
        document.getElementById(
          "nome-gato"
        ).value,

      texto:
        document.getElementById(
          "texto-adocao"
        ).value
    };

    const resposta =
      await fetch(
        "/api/mensagens",
        {
          method: "POST",

          headers: {
            "Content-Type":
              "application/json"
          },

          body:
            JSON.stringify(
              dados
            )
        }
      );

    const json =
      await resposta.json();

    alert(
      json.mensagem
    );

    carregarMensagens();

    form.reset();
  }
);

async function carregarMensagens() {

  const resposta =
    await fetch(
      "/api/mensagens"
    );

  const mensagens =
    await resposta.json();

  const lista =
    document.getElementById(
      "lista-mensagens"
    );

  if (
    mensagens.length === 0
  ) {

    lista.innerHTML =
      "<p>Nenhuma solicitação encontrada.</p>";

    return;
  }

  lista.innerHTML = "";

  mensagens.forEach(
    mensagem => {

      lista.innerHTML += `
        <div class="card p-3 mb-3">

          <strong>
            ${mensagem.nomeAdotante}
          </strong>

          deseja adotar

          <strong>
            ${mensagem.nomeGato}
          </strong>

          <p class="mt-2 mb-0">
            ${mensagem.texto}
          </p>

        </div>
      `;
    }
  );
}

renderizarGatos();
carregarMensagens();