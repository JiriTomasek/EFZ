using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using EFZ.Resources;
using EFZ.WebApplication.Models.Delivery;
using EFZ.WebApplication.Models.Order;

namespace EFZ.WebApplication.Models.Attachment
{
    public class AttachmentVm : BaseVm
    {

        [Display(ResourceType = typeof(Labels), Name = "valDelivery")]
        public long? DeliveryId { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valDeliveryNumber")]
        public string DeliveryNumber { get; set; }
        [Display(ResourceType = typeof(Labels), Name = "valOrderNumber")]
        public string OrderNumber { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool IsCompleted { get; set; }
        public DeliveryVm Delivery { get; set; }


        [Display(ResourceType = typeof(Labels), Name = "valCustomer")]
        public long CustomerId { get; set; }

        [Display(ResourceType = typeof(Labels), Name = "valOrder")]
        public long OrderId { get; set; }

        public static Entities.Entities.Attachment MapToEntityModel(AttachmentVm source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<AttachmentVm, Entities.Entities.Attachment>()
                
                .ForMember(x => x.Delivery, y => y.MapFrom(z => DeliveryVm.MapToEntityModel(z.Delivery)))
            );
            var mapper = config.CreateMapper();
            var destination = mapper.Map<Entities.Entities.Attachment>(source);
            return destination;
        }

        public static AttachmentVm MapToViewModel(Entities.Entities.Attachment source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Entities.Entities.Attachment, AttachmentVm>()
                .ForMember(x => x.Delivery, y => y.MapFrom(z => DeliveryVm.MapToViewModel(z.Delivery)))
            );

            var mapper = config.CreateMapper();
            var destination = mapper.Map<AttachmentVm>(source);
            return destination;
        }

    }
}