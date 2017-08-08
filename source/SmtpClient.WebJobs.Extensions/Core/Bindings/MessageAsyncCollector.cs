using Microsoft.Azure.WebJobs;
using Smtp.WebJobs.Extensions.Client;
using Smtp.WebJobs.Extensions.Config;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Smtp.WebJobs.Extensions.Bindings
{
    internal class MessageAsyncCollector : IAsyncCollector<Message>
    {
        private readonly SmtpConfiguration _config;
        private readonly SmtpAttribute _attribute;
        private readonly ISmtpClient _client;
        private readonly Collection<Message> _messages = new Collection<Message>();

        public MessageAsyncCollector(SmtpConfiguration config, SmtpAttribute attribute, ISmtpClient client)
        {
            _config = config;
            _attribute = attribute;
            _client = client;
        }

        public Task AddAsync(Message item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            //SmtpHelpers.DefaultMessageProperties(item, _config, _attribute);

            //if (!SmtpHelpers.IsToValid(item))
            //{
            //    throw new InvalidOperationException("A 'To' address must be specified for the message.");
            //}

            //if (item.From == null || string.IsNullOrEmpty(item.From.Address))
            //{
            //    throw new InvalidOperationException("A 'From' address must be specified for the message.");
            //}

            _messages.Add(item);
            
            return Task.CompletedTask;
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var message in _messages)
            {
                await _client.SendMessageAsync(message.From, message.To, message.Subject, message.Body);
            }
        }
    }
}
