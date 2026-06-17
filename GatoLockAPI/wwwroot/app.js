const gatos = [
  {
    id: 1,
    nome: "Virgil",
    idade: "2 anos",
    descricao: "Muito carinhoso e dorminhoco.",
    imagem: "assets/imagens/virgil.jpeg"
  },
  {
    id: 2,
    nome: "Nyx",
    idade: "1 ano",
    descricao: "Brincalhona e cheia de energia.",
    imagem: "assets/imagens/nyx.jpeg"
  },
  {
    id: 3,
    nome: "De Selby",
    idade: "3 anos",
    descricao: "Calmo, observador e amoroso.",
    imagem: "assets/imagens/deSelby.jpeg"
  }
];

const listaGatos = document.getElementById("lista-gatos");

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

    try {

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

      document.getElementById(
        "grpc-status"
      ).innerHTML = "✅ Requisição enviada via Gateway gRPC";

      carregarFila();

      form.reset();

      const modal =
        bootstrap.Modal.getInstance(
          document.getElementById(
            "modalAdocao"
          )
        );

      if (modal) {
        modal.hide();
      }

      carregarFila();

    } catch (erro) {

      console.error(
        erro
      );

      alert(
        "Erro ao enviar solicitação."
      );
    }
  }
);

async function carregarFila() {

  try {

    const resposta =
      await fetch(
        "/api/mensagens/fila"
      );

    const fila =
      await resposta.json();

    const container =
      document.getElementById(
        "fila-adocao"
      );

    if (!container) {
      return;
    }

    if (fila.length === 0) {

      container.innerHTML =
        "<p>Nenhuma solicitação na fila.</p>";

      return;
    }

    container.innerHTML = "";

    fila.forEach(item => {

      container.innerHTML += `
        <div class="card p-3 mb-3">

          <strong>
            #${item.id}
          </strong>

          <br>

          👤 ${item.nomeAdotante}

          <br>

          🐱 ${item.nomeGato}

          <br>

          ⏳ Status:
          ${item.status}

        </div>
      `;
    });

  } catch (erro) {

    console.error(
      "Erro ao carregar fila:",
      erro
    );
  }
}

async function carregarMensagens() {

  try {

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

    if (!lista) {
      return;
    }

    if (
      mensagens.length === 0
    ) {

      lista.innerHTML =
        "<p>Nenhuma solicitação processada.</p>";

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

            <span class="badge bg-success mt-2">
              Processada
            </span>

          </div>
        `;
      }
    );

  } catch (erro) {

    console.error(
      "Erro ao carregar mensagens:",
      erro
    );
  }
}

/* ===========================
   SIGNALR
=========================== */

const connection =
  new signalR
    .HubConnectionBuilder()
    .withUrl("/hubs/solicitacoes")
    .build();

connection.on(
  "filaAtualizada",
  () => {

    carregarFila();
  }
);

connection.on(
  "solicitacaoProcessando",
  (solicitacao) => {

    const container =
      document.getElementById(
        "solicitacao-processando"
      );

    container.innerHTML = `
      <div class="card p-3 border-warning">

        <h5>
          ⏳ Processando Solicitação
        </h5>

        <strong>
          ${solicitacao.nomeAdotante}
        </strong>

        deseja adotar

        <strong>
          ${solicitacao.nomeGato}
        </strong>

        <p class="mt-2 mb-0">
          ${solicitacao.texto}
        </p>

      </div>
    `;

    carregarFila();
  }
);

connection.on(
  "solicitacaoConcluida",
  () => {

    document.getElementById(
      "solicitacao-processando"
    ).innerHTML =
      "<p>Nenhuma solicitação sendo processada.</p>";

    carregarFila();

    carregarMensagens();
  }
);

connection
  .start()
  .then(() => {

    console.log(
      "SignalR conectado!"
    );

  })
  .catch(console.error);

/* ===========================
   INICIALIZAÇÃO
=========================== */

renderizarGatos();

carregarFila();

carregarMensagens();