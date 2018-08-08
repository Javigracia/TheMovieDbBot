using System.Collections.Generic;
using AdaptiveCards;
using Microsoft.Bot.Connector;

namespace TheMovieDbBot.Resources
{
    public static class FactoryDialogControls
    {
        internal static AdaptiveCard GetAdaptativeCard(List<CardElement> cardElements)
        {
            return new AdaptiveCard()
            {
                Body = cardElements
            };
        }

        internal static TextBlock GetLargeLightTextBlock(string text)
        {
            return new TextBlock()
            {
                Text = text,
                Size = TextSize.ExtraLarge,
                Weight = TextWeight.Lighter,
                Wrap = true
            };
        }

        internal static TextBlock GetMediumNormalTextBlock(string text)
        {
            return new TextBlock()
            {
                Text = text,
                Size = TextSize.Large,
                Weight = TextWeight.Normal,
                Wrap = true
            };
        }

        //Not supported in Facebook
        internal static ChoiceSet GetMediumNormalTextBlock(List<Choice> choices)
        {
            return new ChoiceSet()
            {
                Id = "options",
                Choices = choices,
                IsMultiSelect = false
            };
        }

        internal static Choice GetChoice(string title, string value)
        {
            return new Choice()
            {
                Title = title,
                Value = value
            };
        }

        internal static ReceiptCard GetReceiptCard(string title, IList<ReceiptItem> items,
            IList<Microsoft.Bot.Connector.Fact> facts, CardAction tap, string total,
            string tax, string vat, IList<CardAction> buttons)
        {
            var receiptCard = new ReceiptCard
            {
                Title = title,
                Items = items,
                Tap = tap,
                Total = total,
                Tax = tax,
                Vat = vat,
                Buttons = buttons
            };
            return receiptCard;
        }

        internal static ReceiptItem GetReceiptItem(string title, string subtitle,
            string price, string quantity, string text, CardImage cardImage)
        {
            return new ReceiptItem
            {
                Title = title,
                Subtitle = subtitle,
                Price = price,
                Quantity = quantity,
                Text = text,
                Image = cardImage
            };
        }

        internal static Microsoft.Bot.Connector.Fact GetFact(string key, string value)
        {
            return new Microsoft.Bot.Connector.Fact
            {
                Key = key,
                Value = value
            };
        }

        internal static ThumbnailCard GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage,
            IList<CardAction> buttons)
        {
            var thumbnailCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Buttons = buttons
            };
            if (cardImage != null) thumbnailCard.Images = new List<CardImage> { cardImage };
            return thumbnailCard;
        }

        internal static HeroCard GetHeroCard(string title, string subtitle, string text, CardImage cardImage,
            IList<CardAction> buttons)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Buttons = buttons
            };

            if (cardImage != null) heroCard.Images = new List<CardImage> { cardImage };
            return heroCard;
        }

        internal static CardAction GetCardAction(string title, string description = null, object value = null)
        {
            if (value == null)
            {
                value = title;
            }
            
            var action = new CardAction
            {
                Title = title,
                DisplayText = description,
                Value = value,
                Type = ActionTypes.ImBack
            };
            return action;
        }

        internal static CardAction GetSuggestedAction(string title)
        {
            return GetCardAction(title, "", title);
        }
    }
}

