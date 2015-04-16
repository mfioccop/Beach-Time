using System;
using NUnit.Framework;
using BeachTime.Data;

namespace BeachTime.Data.Tests {
	using System.Collections.Generic;
	using System.Linq;

	[TestFixture]
	public class TestFileRepository : DbTestFixture {
		private IFileRepository fileRepository;

		private FileInfo fileInfo;

		[SetUp]
		public override void SetUp() {
			base.SetUp();
			fileRepository = new FileRepository(dbContext.ConnectionStringName);
			fileInfo = new FileInfo {
				             Description = "A file description.",
				             Path = @"c:\path\to\file\on\server\file.pdf",
				             Title = "File title",
				             UserId = 1
			             };
		}

		[Test]
		public void TestCreateId() {
			fileRepository.Create(fileInfo);
			Assert.True(fileInfo.FileId > 0);
		}

		[Test]
		public void TestCreateFind() {
			fileRepository.Create(fileInfo);
			var actual = fileRepository.FindByFileId(fileInfo.FileId);
			AssertEx.PropertyValuesAreEquals(fileInfo, actual);
		}

		[Test]
		public void TestSingleFindUserId() {
			fileRepository.Create(fileInfo);
			var actual = fileRepository.FindByUserId(fileInfo.UserId).Single();
			AssertEx.PropertyValuesAreEquals(fileInfo, actual);
		}

		[Test]
		public void TestMultipleFindUserId() {
			var fileInfo2 = new FileInfo {
				                             Description = fileInfo.Description,
				                             Path = @"c:\another\path\",
				                             Title = "title",
				                             UserId = 1
			                             };
			fileRepository.Create(fileInfo);
			fileRepository.Create(fileInfo2);
			var files = fileRepository.FindByUserId(1);
			var fileInfos = files as IList<FileInfo> ?? files.ToList();
			AssertEx.PropertyValuesAreEquals(2, fileInfos.Count);
			var actualFile = fileInfos.Single(f => f.Title == "File title");
			var actualFile2 = fileInfos.Single(f => f.Title == "title");
			AssertEx.PropertyValuesAreEquals(fileInfo, actualFile);
			AssertEx.PropertyValuesAreEquals(fileInfo2, actualFile2);
		}

		[Test]
		public void TestUpdate() {
			fileRepository.Create(fileInfo);
			fileInfo.Title = "changed";
			fileInfo.Path = "also changed";
			fileInfo.Description = "this too";
			fileInfo.UserId = 2;
			fileRepository.Update(fileInfo);
			var actual = fileRepository.FindByFileId(fileInfo.FileId);
			AssertEx.PropertyValuesAreEquals(fileInfo, actual);
		}

		[Test]
		public void TestUpdateConcurrency() {
			fileRepository.Create(fileInfo);
			var info1 = fileRepository.FindByFileId(fileInfo.FileId);
			var info2 = fileRepository.FindByFileId(fileInfo.FileId);
			info1.Title = "change1";
			info2.Title = "change2";
			fileRepository.Update(info1);
			fileRepository.Update(info2);
			var actual = fileRepository.FindByFileId(fileInfo.FileId);
			Assert.AreEqual("change1", actual.Title);
		}

		[Test]
		public void TestDelete() {
			fileRepository.Create(fileInfo);
			AssertEx.PropertyValuesAreEquals(fileInfo, fileRepository.FindByFileId(fileInfo.FileId));
			fileRepository.Delete(fileInfo);
			Assert.Null(fileRepository.FindByFileId(fileInfo.FileId));
		}
	}
}
