package com.goat.deathnote.domain.member.service;

import com.goat.deathnote.domain.garden.dto.GardenDetailsDto;
import com.goat.deathnote.domain.garden.entity.Garden;
import com.goat.deathnote.domain.garden.repository.GardenRepository;
import com.goat.deathnote.domain.member.dto.MemberDetailResDto;
import com.goat.deathnote.domain.member.dto.UpdateMemberDto;
import com.goat.deathnote.domain.member.entity.Member;
import com.goat.deathnote.domain.member.entity.MemberRole;
import com.goat.deathnote.domain.member.entity.SocialProvider;
import com.goat.deathnote.domain.member.repository.MemberRepository;
import com.goat.deathnote.domain.soul.dto.SoulDetailsDto;
import com.goat.deathnote.domain.soul.entity.Soul;
import com.goat.deathnote.domain.soul.repository.SoulRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import javax.transaction.Transactional;
import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class MemberService {

    private final MemberRepository memberRepository;
    private final SoulRepository soulRepository;
    private final GardenRepository gardenRepository;

    public Member signUp(String email, String nickname) {
        // 이메일 중복 체크
        Member member = memberRepository.findByEmail(email).orElse(null);
        if (member != null) {
            throw new RuntimeException("이미 등록된 이메일입니다.");
        }
        // 닉네임 중복 체크
        if (memberRepository.findByNickname(nickname) != null) {
            throw new RuntimeException("이미 등록된 닉네임입니다.");
        }
        member = Member.builder()
                .name("테스트")
                .email(email)
                .role(MemberRole.USER)
                .provider(SocialProvider.GOOGLE)
                .nickname(nickname)
                .level(1L)
                .gold(0L)
                .progress(0L)
                .build();
        return memberRepository.save(member);
    }
    public Member login(String email) {
        Member member = memberRepository.findByEmail(email).orElseThrow();
        if (member != null) {
            return member;
        }
        if (member == null){
            throw new RuntimeException("해당 이메일로 등록된 멤버가없습니다.");
        }
        return null;
    }
    public List<Member> getAllMembers() {
        return memberRepository.findAll();
    }

    public Member getMerberById(Long memberId){
        return memberRepository.findById(memberId).orElseThrow();
    }
//    public MemberDetailResDto getMemberWithSoul(Long memberId) {
//        Member member = memberRepository.findById(memberId).orElseThrow();
//
//        List<Soul> souls = soulRepository.findByMemberId(member.getId());
//        List<SoulDetailsDto> soulDetails = new ArrayList<>();
//        for (Soul s : souls) {
//            SoulDetailsDto soulDetailsDto = new SoulDetailsDto(s);
//            soulDetails.add(soulDetailsDto);
//        }
//        List<Garden> gardens = gardenRepository.findByMemberId(member.getId());
//        List<GardenDetailsDto> gardenDetails = new ArrayList<>();
//        for (Garden g : gardens) {
//            GardenDetailsDto gardenDetailsDto = new GardenDetailsDto(g);
//            gardenDetails.add(gardenDetailsDto);
//        }
//
//        MemberDetailResDto memberDetailResDto = new MemberDetailResDto(member, soulDetails, gardenDetails);
//        return memberDetailResDto;
//    }
    public MemberDetailResDto getMemberWithSoul(String email) {
        Member member = memberRepository.findByEmail(email).orElseThrow();

        List<Soul> souls = soulRepository.findByMemberId(member.getId());
        List<SoulDetailsDto> soulDetails = new ArrayList<>();
        for (Soul s : souls) {
            SoulDetailsDto soulDetailsDto = new SoulDetailsDto(s);
            soulDetails.add(soulDetailsDto);
        }
        List<Garden> gardens = gardenRepository.findByMemberId(member.getId());
        List<GardenDetailsDto> gardenDetails = new ArrayList<>();
        for (Garden g : gardens) {
            GardenDetailsDto gardenDetailsDto = new GardenDetailsDto(g);
            gardenDetails.add(gardenDetailsDto);
        }

        MemberDetailResDto memberDetailResDto = new MemberDetailResDto(member, soulDetails, gardenDetails);
        return memberDetailResDto;
    }

    public void deleteMember(Long memberId) {
        memberRepository.deleteById(memberId);
    }

    public Member updateNicknameById(Long id, UpdateMemberDto updateMemberDto) {
        Member member = memberRepository.findById(id).orElseThrow();

        if (member != null) {
            member.setNickname(updateMemberDto.getNickname());
            return memberRepository.save(member);
        }

        // 유저를 찾을 수 없을 때 예외처리 (여기에서는 null을 반환하거나 예외를 던질 수 있음)
        return null;
    }

    @Transactional
    public void updateMember(Long memberId, UpdateMemberDto updateMemberDto) {
        Member member = memberRepository.findById(memberId)
                .orElseThrow(() -> new MemberNotFoundException("멤버를 찾지 못했습니다."));

        // 업데이트할 필드들만 업데이트
        if (updateMemberDto.getLevel() != null) {
            member.setLevel(updateMemberDto.getLevel());
        }

        if (updateMemberDto.getGold() != null) {
            member.setGold(updateMemberDto.getGold());
        }

        if (updateMemberDto.getProgress() != null) {
            member.setProgress(updateMemberDto.getProgress());
        }

        if (updateMemberDto.getNickname() != null) {
            member.setNickname(updateMemberDto.getNickname());
        }

        // 저장
        memberRepository.save(member);
    }

}