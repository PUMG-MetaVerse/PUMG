package com.ghs.pumg.dto.request;


import lombok.*;


@Getter
@Setter
@ToString
@NoArgsConstructor
@AllArgsConstructor
public class TitleSetReq {
    private Long userIdx;
    private Long titleIdx;
}
