﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Np.NotesService.Application.Abstractions.Data;
using Np.NotesService.Application.Notes.AddNote;
using Np.NotesService.Application.Notes.RemoveNote;
using Np.NotesService.Application.Notes.GetNote;
using Np.NotesService.Application.Notes.GetNotesFromRoot;
using Np.NotesService.Application.Notes.UpdateNote;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Np.NotesService.Application.Notes.GetOutgoingNotes;
using Np.NotesService.Application.Notes.GetIncomingNotes;

namespace Np.NotesService.Api.Controllers.Notes;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public NotesController(
        ISqlConnectionFactory sqlConnectionFactory,
        ISender sender,
        IMapper mapper)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("root")]
    [Authorize]
    public async Task<ActionResult<List<NoteItem>>> GetNotesFromRoot()
    {
        var identityId = GetUserIdentityId();
        if (identityId == null)
        {
            return Unauthorized();
        }

        var response = await _sender.Send(new GetNotesFromRootQuery(identityId));

        return Ok(response.Value);
    }

    [HttpGet("{id:guid}", Name=nameof(GetNote))]
    [Authorize]
    public async Task<ActionResult<GetNoteResponse>> GetNote(Guid id)
    {
        var identityId = GetUserIdentityId();
        if (identityId == null)
        {
            return Unauthorized();
        }
        var result = await _sender.Send(new GetNoteQuery(id, identityId));

        if (result.IsFailed)
        {
            return NotFound(result.Message);
        }

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> UpdateNote([FromBody] UpdateNoteRequest request, Guid id)
    {
        var identityId = GetUserIdentityId();
        if (identityId == null)
        {
            return Unauthorized();
        }

        var result = await _sender.Send(new UpdateNoteCommand(request.Data, id, identityId));
        if (result.IsFailed && result.Message.Equals(UpdateNoteErrors.NotFound))
        {
            return NotFound(result.Message);
        }
        if (result.IsFailed)
        {
            return StatusCode(500);
        }

        return NoContent();
    }


    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteNote(Guid id)
    {
        var identityId = GetUserIdentityId();
        if (identityId == null)
        {
            return Unauthorized();
        }

        var result = await _sender.Send(new RemoveNoteCommand(id, identityId));

        if (result.IsFailed)
        {
            return StatusCode(500);
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateNote([FromBody] AddNoteRequest request)
    {
        var identityId = GetUserIdentityId();
        if (identityId == null)
        {
            return Unauthorized();
        }
        var result = await _sender.Send(new AddNoteCommand(request.Data, identityId));

        if (result.IsFailed)
        {
            return StatusCode(500);
        }

        return CreatedAtRoute(nameof(GetNote), new { id = result.Value }, result.Value);
    }

    [HttpGet("{outgoingNoteId:guid}/outgoings")]
    public async Task<ActionResult> GetOutgoingRelations(Guid outgoingNoteId)
    {
        var identityId = GetUserIdentityId();
        if (identityId == null)
        {
            return Unauthorized();
        }
        var result = await _sender.Send(new GetOutgoingNotesQuery(outgoingNoteId, identityId));

        if(result.IsFailed && result.Message.Equals(GetOutgoingNotesErrors.NotFound))
        {
            return NotFound(result.Message);
        }
        return Ok(result.Value);
    }

    [HttpGet("{incomingNoteId:guid}/incomings")]
    public async Task<ActionResult> GetIncomingRelations(Guid incomingNoteId)
    {
        var identityId = GetUserIdentityId();
        if (identityId == null)
        {
            return Unauthorized();
        }
        var result = await _sender.Send(new GetIncomingRelationsQuery(incomingNoteId, identityId));
        if (result.IsFailed && result.Message.Equals(GetIncomingNotesErrors.NotFound))
        {
            return NotFound(result.Message);
        }
        return Ok(result.Value);
    }

    private string? GetUserIdentityId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}

