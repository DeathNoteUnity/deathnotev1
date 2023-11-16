package com.goat.deathnote.domain.member.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class UpdateMemberDto {

    private Long level;
    private Long gold;
    private Long progress;
    private String nickname;

}