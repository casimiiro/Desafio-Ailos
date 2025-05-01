using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Movimenta uma conta corrente (crédito ou débito).
        /// </summary>
        /// <param name="command">Dados da movimentação.</param>
        /// <returns>Id do movimento criado.</returns>
        [HttpPost("movimentar")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status200OK, "Movimentação realizada com sucesso", typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentarContaCommand command)
        {
            var resultado = await _mediator.Send(command);
            if (!resultado.Sucesso)
            {
                return BadRequest(new { mensagem = resultado.Mensagem, tipo = resultado.TipoErro });
            }

            return Ok(new { idMovimento = resultado.IdMovimento });
        }

        /// <summary>
        /// Consulta o saldo de uma conta corrente.
        /// </summary>
        /// <param name="numeroConta">Numero da conta corrente.</param>
        /// <returns>Informações do saldo.</returns>
        [HttpGet("saldo/{numeroConta}")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status200OK, "Saldo consultado com sucesso", typeof(ConsultarSaldoResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Conta inválida ou inativa")]
        public async Task<IActionResult> ConsultarSaldo([FromRoute] int numeroConta)
        {
            var resultado = await _mediator.Send(new ConsultarSaldoQuery(numeroConta));

            if (!resultado.Sucesso)
            {
                return BadRequest(new { mensagem = resultado.Mensagem, tipo = resultado.TipoErro });
            }

            return Ok(resultado);
        }
    }
}