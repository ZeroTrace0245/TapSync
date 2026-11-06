# TapSync: NFC Event Access and Safety Management System

## Project Proposal

**Prepared by:** R. M. S. D. Rathnayaka  
**Index:** 3374  
**Date:** 5 November 2026

## 1. Introduction

Large events need a reliable way to register attendees, verify their access rights at entry points, and account for people during an emergency. Paper lists and disconnected tools can slow entry, make access rules difficult to update, and leave organizers without a timely view of attendance or venue conditions.

TapSync is a proposed NFC-based event access and operations system. It links an attendee’s event record to an NFC credential and gives event staff tools to validate entry, manage access zones, monitor activity, and carry out emergency roll calls. The system is designed to continue making access decisions when the venue network is unavailable and synchronize records when connectivity returns.

## 2. Problem Statement

Event organizers often manage registrations, credentials, entry points, staff permissions, and emergency attendance through separate processes. This can cause long queues, duplicate or invalid credentials, slow responses to changing access requirements, and incomplete information during incidents. A centralized system with offline-capable gate checks and a shared operational dashboard can address these problems.

## 3. Aim and Objectives

### Aim

To design and develop an NFC-based event access and safety management system that supports attendee registration, secure entry validation, live operations, and emergency accountability.

### Objectives

- Provide a centralized desktop hub for event setup, attendee records, credential encoding, and staff administration.
- Support attendee registration, spreadsheet import, check-in, and NFC credential assignment.
- Validate credentials at gates against access tiers and zone permissions, including when offline.
- Detect duplicate, revoked, or suspicious credentials and keep an auditable record of access decisions.
- Give organizers a live attendance and zone dashboard with operational alerts.
- Provide an emergency roll-call workflow using the latest available attendance and zone data.
- Evaluate the prototype through functional, usability, offline-operation, and security testing.

## 4. Proposed Solution

TapSync will consist of a desktop administration hub, a gate application, an NFC credential workflow, a backend synchronization service, and an operations dashboard. Organizers create an event and define attendee categories, access zones, and staff roles. Attendees are registered individually or imported in bulk; credentials are then encoded and bound to attendee records.

At entry points, staff tap a credential using a supported NFC reader. The gate application checks cached credential and access-rule data locally, provides an admit or deny result, and records the decision for later synchronization. The administration hub and dashboard provide event staff with attendance summaries, access activity, zone information, and roll-call tools.

## 5. Project Scope

### Included

- Event and attendee registration, including bulk import and check-in.
- NFC credential encoding, assignment, replacement, and revocation.
- Access tiers, zone permissions, and gate validation.
- Offline gate operation and later synchronization.
- Staff accounts, role-based permissions, and audited supervisor overrides.
- Attendance dashboard, zone monitoring, operational alerts, and emergency roll call.
- Basic attendance and access reports, technical documentation, and prototype evaluation.

## 6. Core Features

### 6.1 Centralized Event Administration

The administration hub is the main workspace for organizers. An authorized organizer can create an event, enter its dates and venue details, set up entry gates and access zones, and configure event rules. The hub also provides a single place to view and update attendee records and staff accounts. Keeping these tasks together helps prevent different teams from working from separate or outdated lists.

### 6.2 Attendee Registration and Check-in

Registration staff can add attendees individually or import a spreadsheet of registrations. The system checks imported information for missing or invalid fields and reports rows that need correction. Staff can search and update attendee records, mark an attendee as checked in, and see their registration status. This feature helps organizers prepare the attendee list before the event and handle exceptions at the venue.

### 6.3 NFC Credential Management

Each registered attendee can be assigned an NFC tag or wristband linked to their event record. Staff use the credential management workflow to encode and verify a credential, see which attendee it belongs to, and replace or revoke it if it is lost or damaged. Revocation prevents an old credential from being accepted after gate devices receive the updated information. The attendee record remains available when a replacement credential is issued.

### 6.4 Gate Access Validation

Gate staff use the gate application with a supported NFC reader. When an attendee taps their credential, the application checks whether it is recognized, active, and permitted at that gate. It then gives staff a clear admit or deny result and records the time, gate, and decision. This reduces reliance on visual checks and paper lists while giving supervisors a record to review if an entry is disputed.

### 6.5 Offline-first Operation and Synchronization

Before an event, gate devices download the attendee credentials and access rules needed for their assigned gates. They can continue checking credentials if internet or venue network access is interrupted, while saving access decisions on the device. When the connection returns, the device sends those records to the central system and receives updates. This feature keeps entry operations running during outages and brings the event records back into sync afterward.

### 6.6 Access Tiers, Zones, and Entry Rules

Organizers can define attendee categories such as general admission, VIP, staff, or press, then assign each category permissions for specific gates, venue zones, or event sessions. Gate devices apply those permissions when a credential is tapped. Organizers can also configure entry rules, such as limiting re-entry or preventing use of a single-entry pass more than once. Rule changes are distributed to connected devices and applied locally by offline devices when they next synchronize.

### 6.7 Credential Integrity and Supervisor Overrides

The system checks for revoked credentials, repeated use, and suspicious activity, and shows gate staff the reason when access is denied. When a legitimate attendee cannot use their credential, an authorized supervisor can record a manual admission with a reason. The override is saved with the supervisor’s account, time, and gate so it can be reviewed later. These controls help staff resolve problems while preserving an accountable record of exceptions.

### 6.8 Live Operations Dashboard and Alerts

The dashboard gives authorized organizers a current view of check-ins, gate activity, and attendance by venue zone. It can highlight unusual activity or configured capacity concerns so staff can investigate and respond. The dashboard uses records received from connected devices; activity from offline gates appears after synchronization. This gives the operations team a shared view during the event without implying that disconnected devices provide live updates.

### 6.9 Emergency Roll Call

If an incident occurs, authorized staff can start a roll call for the affected venue or zone. The system uses the latest available gate and zone records to provide an attendee list for staff to check, and lets responders mark people as accounted for, missing, or requiring assistance. The roll-call view displays when its underlying attendance data was last updated, helping coordinators understand its limits and assign follow-up checks. It supports emergency coordination but does not replace instructions from emergency services.

### 6.10 Audit Trail and Attendance Reports

TapSync records important actions, including credential issuance or revocation, access decisions, staff changes, and supervisor overrides. Authorized organizers can review this history and generate basic attendance and access reports for event planning and post-event review. Reports can summarize check-ins by time, gate, attendee category, or zone where the collected records support that view. The audit trail helps explain what happened and when, while role permissions limit who can view or change sensitive records.

## 7. Intended Users

- Event organizers and administrators
- Registration and box-office staff
- Gate and ground staff
- Security supervisors
- Emergency response coordinators

## 8. Methodology

The project will use an iterative development approach. Requirements will be confirmed with representative event roles, followed by system and data design. A working prototype will be developed in increments, beginning with event setup, registration, credential encoding, and gate validation. Dashboard, synchronization, and emergency workflows will then be integrated. Each increment will be reviewed against defined acceptance criteria and revised using feedback from trial users.

## 9. Proposed Timeline

| Phase | Activities | Duration |
|---|---|---|
| 1. Requirements and planning | Confirm use cases, requirements, constraints, and acceptance criteria | 2 weeks |
| 2. Design | Architecture, data model, NFC credential format, and interface prototypes | 3 weeks |
| 3. Core prototype | Registration, staff setup, credential encoding, and gate validation | 5 weeks |
| 4. Operations features | Dashboard, offline synchronization, access rules, and roll call | 5 weeks |
| 5. Evaluation and delivery | Integration, user evaluation, corrections, documentation, and demonstration | 3 weeks |

**Estimated total:** 18 weeks. The schedule will be refined after requirements and hardware availability are confirmed.

## 10. Deliverables

- Requirements specification and system design documentation
- Desktop administration hub prototype
- NFC credential encoding and management workflow
- Gate validation prototype with offline operation
- Operations dashboard and emergency roll-call workflow
- Demonstration setup and evaluation results
- User and technical documentation

## 11. Evaluation Criteria

- Correct admission and denial for configured access rules.
- Gate validation remains available during a network interruption and synchronizes records afterward.
- Credential replacement and revocation are reflected at gates after synchronization.
- Authorized users can complete registration, credential, and roll-call tasks using the documented workflows.
- Access decisions and administrative overrides include operator, time, location, and outcome where applicable.
- User evaluation and test results are documented, including identified limitations.

## 12. Risks and Mitigation

| Risk | Mitigation |
|---|---|
| NFC reader or tag incompatibility | Confirm supported hardware early and test representative devices during prototyping. |
| Network outages or delayed synchronization | Cache required access data locally and test recovery after offline use. |
| Credential cloning or misuse | Use secure credential options where available, maintain revocation controls, and audit anomalies. |
| Personal information exposure | Limit collected information, apply role-based access, and document retention and access controls. |
| Scope growth | Use the included and excluded scope as the baseline for project reviews. |

## 13. Conclusion

TapSync will provide an integrated prototype for event registration, NFC-based access control, live operational awareness, and emergency accountability. Its focus is secure and dependable event operations, with payments and retail functions outside the project scope.
