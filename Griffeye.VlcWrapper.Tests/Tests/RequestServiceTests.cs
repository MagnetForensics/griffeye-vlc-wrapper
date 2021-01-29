﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Griffeye.VlcWrapper.Tests.AutoData;
using Griffeye.VlcWrapper.Tests.AutoData.TestModels;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Griffeye.VlcWrapper.Tests.Tests
{
    public class RequestServiceTests
    {
        public class IsQuitMessageShould
        {
            [Theory, AutoNSubstituteData]
            public void ReturnTrueIfQuitMessage(Stream inStream, Stream outStream, Quit message,
                IMediaPlayer mediaPlayer, RequestService sut)
            {
                sut.IsQuitMessage(message).Should().BeTrue();
            }
        }

        public class CanHandleMessageShould
        {
            [BaseRequestMessageData(MessageType.Play)]
            [BaseRequestMessageData(MessageType.Pause)]
            [BaseRequestMessageData(MessageType.LocalFileStreamDisconnect)]
            [BaseRequestMessageData(MessageType.PlaybackSpeed)]
            [BaseRequestMessageData(MessageType.StepForward)]
            [BaseRequestMessageData(MessageType.StepBack)]
            [BaseRequestMessageData(MessageType.Seek)]
            [BaseRequestMessageData(MessageType.Load)]
            [BaseRequestMessageData(MessageType.LocalFileStreamConnect)]
            [BaseRequestMessageData(MessageType.CreateSnapshot)]
            [BaseRequestMessageData(MessageType.Volume)]
            [BaseRequestMessageData(MessageType.Mute)]
            [BaseRequestMessageData(MessageType.EnableImageOption)]
            [BaseRequestMessageData(MessageType.EnableHardwareDecoding)]
            [BaseRequestMessageData(MessageType.SetImageOption)]
            [BaseRequestMessageData(MessageType.AddMediaOption)]
            [BaseRequestMessageData(MessageType.SetMediaTrack)]
            public void ReturnTrueIfValidMessageType(Stream outStream, BaseRequest message,
                IMediaPlayer mediaPlayer, RequestService sut)
            {
                mediaPlayer.CreateSnapshot(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns(true);
                sut.CanHandleMessage(mediaPlayer, message, outStream).Should()
                    .BeTrue("Because it is a valid message.");
            }

            /* Invalid cast message */

            [BaseRequestMessageData(MessageType.InvalidCast)]
            public void ReturnFalseIfInvalidMessage(IMediaPlayer mediaPlayer, Stream inStream,
                Stream outStream, BaseRequest message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream).Should()
                    .BeFalse("Because it's an invalid message.");
            }

            /* Execute Media Player Commands */

            [Theory, BaseRequestMessageData(MessageType.Play)]
            public void CallPlayOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, BaseRequest message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).Play();
            }

            [Theory, BaseRequestMessageData(MessageType.Pause)]
            public void CallPauseOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, BaseRequest message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).Pause();
            }

            [Theory, BaseRequestMessageData(MessageType.LocalFileStreamDisconnect)]
            public void CallDisconnectLocalFileStreamOnIMediaPlayer(
                [Frozen] IMediaPlayer mediaPlayer, Stream outStream, BaseRequest message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).DisconnectLocalFileStream();
            }

            [Theory, BaseRequestMessageData(MessageType.PlaybackSpeed)]
            public void CallSetPlaybackSpeedOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, SetPlaybackSpeed message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).SetPlaybackSpeed(message.Speed);
            }

            [Theory, BaseRequestMessageData(MessageType.StepForward)]
            public void CallStepForwardOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, BaseRequest message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).StepForward();
            }

            [Theory, BaseRequestMessageData(MessageType.StepBack)]
            public void CallStepBackOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, BaseRequest message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).StepBack();
            }

            [Theory, BaseRequestMessageData(MessageType.Seek)]
            public void CallSeekMessageOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, Seek message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).Seek(message.Position);
            }

            [Theory, AutoNSubstituteData]
            public void CallLoadMediaOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, Load message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).LoadMedia(message.Type, message.FileToLoad, message.StartPosition, message.StopPosition);
            }

            [Theory, AutoNSubstituteData]
            public void CallConnectLocalFileStreamOnIMediaPlayer(
                [Frozen] IMediaPlayer mediaPlayer, Stream outStream, LocalFileStreamConnect message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).ConnectLocalFileStream(message.PipeName);
            }

            [Theory, AutoNSubstituteData]
            public void CallCreateSnapshotOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, CreateSnapshot message, RequestService sut)
            {
                mediaPlayer.CreateSnapshot(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns(true);
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).CreateSnapshot(message.NumberOfVideoOutput, message.Width, message.Height, message.FilePath);
            }

            [Theory, AutoNSubstituteData]
            public void CallSetVolumeOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, SetVolume message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).SetVolume(message.Volume);
            }

            [Theory, AutoNSubstituteData]
            public void CallSetMuteOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, SetMute message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).SetMute(message.IsMuted);
            }

            [Theory, AutoNSubstituteData]
            public void CallSetMediaTrackOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, SetMediaTrack message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).SetMediaTrack(message.TrackType, message.TrackId);
            }

            [Theory, AutoNSubstituteData]
            public void CallEnableImageOptionsOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, EnableImageOptions message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).EnableImageOptions(message.Enable);
            }

            [Theory, AutoNSubstituteData]
            public void CallEnableHardwareDecodingOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, EnableHardwareDecoding message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).EnableHardwareDecoding(message.Enable);
            }

            [Theory, ImageOptionData]
            public void CallAddMediaOptionOnIMediaPlayer([Frozen] IMediaPlayer mediaPlayer,
                Stream outStream, AddMediaOption message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                mediaPlayer.Received(1).AddMediaOption(message.Option);
            }

            /* Use Response Service For Execute Result Response Messages */

            [Theory, AutoNSubstituteData]
            public void CallReturnResultResponseOnIResponseServiceForLoad([Frozen] IResponseService responseService,
                IMediaPlayer mediaPlayer, Stream outStream, Load message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                responseService.Received(1).ReturnResultResponse(outStream, message, true);
            }

            [Theory, AutoNSubstituteData]
            public void CallReturnResultResponseOnIResponseServiceForLocalFileStreamConnect([Frozen] IResponseService responseService,
                IMediaPlayer mediaPlayer, Stream outStream, LocalFileStreamConnect message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                responseService.Received(1).ReturnResultResponse(outStream, message, true);
            }

            [Theory, AutoNSubstituteData]
            public void CallReturnResultResponseOnIResponseServiceForCreateSnapshot([Frozen] IResponseService responseService,
                IMediaPlayer mediaPlayer, Stream outStream, CreateSnapshot message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                responseService.Received(1).ReturnResultResponse(outStream, message, Arg.Any<bool>());
            }

            /* Use Response Service For Execute Empty Response Messages */

            [BaseRequestMessageData(MessageType.Play)]
            [BaseRequestMessageData(MessageType.Pause)]
            [BaseRequestMessageData(MessageType.LocalFileStreamDisconnect)]
            [BaseRequestMessageData(MessageType.StepForward)]
            [BaseRequestMessageData(MessageType.StepBack)]
            [BaseRequestMessageData(MessageType.Seek)]
            [BaseRequestMessageData(MessageType.EnableImageOption)]
            [BaseRequestMessageData(MessageType.EnableHardwareDecoding)]
            [BaseRequestMessageData(MessageType.SetImageOption)]
            [BaseRequestMessageData(MessageType.AddMediaOption)]
            [BaseRequestMessageData(MessageType.SetMediaTrack)]
            public void CallReturnEmptyResponseOnIResponseService([Frozen] IResponseService responseService,
                IMediaPlayer mediaPlayer, Stream outStream, BaseRequest message, RequestService sut)
            {
                sut.CanHandleMessage(mediaPlayer, message, outStream);
                responseService.Received(1).ReturnEmptyResponse(outStream, message);
            }
        }
    }
}